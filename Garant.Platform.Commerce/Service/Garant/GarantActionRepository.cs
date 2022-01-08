using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Entities.Commerce;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Commerce.Service.Garant
{
    /// <summary>
    /// Класс реализует методы репозитория событий Гаранта.
    /// </summary>
    public class GarantActionRepository : IGarantActionRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public GarantActionRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод получит дату создания сделки.
        /// </summary>
        /// <param name="userId">Id владельца предмета сделки.</param>
        /// <returns>Дату создания сделки.</returns>
        public async Task<DateTime> GetOwnerIdItemDealAsync(string userId)
        {
            try
            {
                var result = await _postgreDbContext.Deals
                    .Where(d => d.UserId.Equals(userId))
                    .Select(d => d.DateCreate)
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет Id пользователя, который создал заказ.
        /// </summary>
        /// <param name="orderId">Id заказа.</param>
        /// <returns>Id пользователя.</returns>
        public async Task<string> GetUserIdCreatedOrderAsync(long orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    return null;
                }

                var userId = await _postgreDbContext.Orders
                    .Where(o => o.OrderId == orderId)
                    .Select(o => o.UserId)
                    .FirstOrDefaultAsync();

                return userId;
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
        /// Метод найдет телефон и почту пользователя создавшего заказ.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<FindUserPhoneEmailCreatedOrderOutput> FindUserEmailPhoneCreatedOrderAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                var result = await _postgreDbContext.Users
                    .Where(u => u.Id.Equals(userId))
                    .Select(u => new FindUserPhoneEmailCreatedOrderOutput
                    {
                        Email = u.Email,
                        Phone = u.PhoneNumber
                    })
                    .FirstOrDefaultAsync();

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
        /// Метод найдет все данные заказа по его Id.
        /// </summary>
        /// <param name="orderId">Id заказа в сервисе Гарант.</param>
        /// <returns>Данные заказа.</returns>
        public async Task<OrderEntity> GetOrderByIdAsync(long orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    return null;
                }

                var result = await _postgreDbContext.Orders.FirstOrDefaultAsync(o => o.OriginalId == orderId);

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
    }
}
