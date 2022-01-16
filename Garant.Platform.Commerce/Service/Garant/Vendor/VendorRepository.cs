using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Commerce.Abstraction.Garant.Vendor;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Commerce.Output;
using Garant.Platform.Models.Entities.Commerce;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Commerce.Service.Garant.Vendor
{
    /// <summary>
    /// Сервис реализует методы репозитория Гаранта со стороны продавца для работы с БД.
    /// </summary>
    public sealed class VendorRepository : IVendorRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;

        public VendorRepository(PostgreDbContext postgreDbContext, IUserRepository userRepository)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Метод создаст новую сделку.
        /// </summary>
        /// <param name="itemDealId">Id предмета сделки.</param>
        /// <param name="userId">Id пользователя, который создал сделку.</param>
        /// <returns>Данные сделки.</returns>
        public async Task<DealOutput> CreateDealAsync(long itemDealId, string userId)
        {
            try
            {
                long dealId = 1;

                var lastDealId = await _postgreDbContext.Deals
                    .OrderBy(o => o.DealId)
                    .Select(d => d.DealId)
                    .LastOrDefaultAsync();

                if (lastDealId <= 0)
                {
                    lastDealId = dealId;
                }

                else
                {
                    lastDealId++;
                }

                // Создаст сделку.
                var addDeal = new DealEntity
                {
                    DealId = lastDealId,
                    DealItemId = itemDealId,
                    UserId = userId,
                    IsCompletedDeal = false
                };

                await _postgreDbContext.Deals.AddAsync(addDeal);
                await _postgreDbContext.SaveChangesAsync();

                // Добавит этапы сделки.
                var dealIterations = InitIterationsDeal(lastDealId);

                // Добавит список этапов сделки.
                await _postgreDbContext.DealIterations.AddRangeAsync(dealIterations);
                await _postgreDbContext.SaveChangesAsync();

                var iterations = new List<DealIterationOutput>();

                // Приведет список этапов сделки к выходному типу.
                foreach (var iteration in dealIterations)
                {
                    if (iteration != null)
                    {
                        var jsonString = JsonSerializer.Serialize(iteration);
                        var newIteration = JsonSerializer.Deserialize<DealIterationOutput>(jsonString);
                        iterations.Add(newIteration);
                    }
                }

                var result = new DealOutput
                {
                    DealId = addDeal.DealId,
                    DealItemId = addDeal.DealItemId,
                    UserId = addDeal.UserId,
                    DealIterations = iterations
                };

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод инициализирует список этапов сделки.
        /// </summary>
        /// <param name="dealId">Id сделки.</param>
        /// <returns>Список этапов сделки.</returns>
        private List<DealIterationEntity> InitIterationsDeal(long dealId)
        {
            var result = new List<DealIterationEntity>
            {
                new DealIterationEntity
                {
                    NumberIteration = 1,
                    IterationName = "Холдирование суммы",
                    Position = 1,
                    DealId = dealId,
                    IterationDetail = "Необходимо для подтверждения покупательской способности"
                },

                new DealIterationEntity
                {
                    NumberIteration = 2,
                    IterationName = "Согласование этапов сделки",
                    Position = 2,
                    DealId = dealId,
                    IterationDetail = "Перед согласованием договора следует договориться об этапах"
                },

                new DealIterationEntity
                {
                    NumberIteration = 3,
                    IterationName = "Согласование договора",
                    Position = 3,
                    DealId = dealId,
                    IterationDetail = "Согласование всех деталей договора"
                },

                new DealIterationEntity
                {
                    NumberIteration = 4,
                    IterationName = "Оплата и исполнение этапов сделки",
                    Position = 4,
                    DealId = dealId,
                    IterationDetail = "Получение оплаты, исполнение продажи каждого этапа"
                }
            };

            return result;
        }

        /// <summary>
        /// Метод проверит существование открытой сделки с таким предметом сделки.
        /// </summary>
        /// <param name="itemDealId">Id предмета сделки.</param>
        /// <param name="userId">Id пользователя, который создал сделку.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<bool> CheckDealByItemDealIdAsync(long itemDealId, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                var result = await _postgreDbContext.Deals
                    .Where(d => d.DealItemId == itemDealId
                                && d.IsClose == false
                                && d.UserId.Equals(userId))
                    .FirstOrDefaultAsync();

                return result != null;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }
    }
}
