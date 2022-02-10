using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Request.Output;

namespace Garant.Platform.Services.Request
{
    public sealed class RequestService : IRequestService
    {
        private readonly IFranchiseRepository _franchiseRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly PostgreDbContext _postgreDbContext;

        public RequestService(IFranchiseRepository franchiseRepository, IBusinessRepository businessRepository, PostgreDbContext postgreDbContext)
        {
            _franchiseRepository = franchiseRepository;
            _businessRepository = businessRepository;
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод создаст заявку франшизы.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="city">Город.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="franchiseId">Id франшизы, по которой оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        public async Task<RequestFranchiseOutput> CreateRequestFranchiseAsync(string userName, string phone, string city, string account, long franchiseId)
        {
            try
            {
                var result = await _franchiseRepository.CreateRequestFranchiseAsync(userName, phone, city, account, franchiseId);

                if (result != null)
                {
                    result.IsSuccessCreatedRequest = true;
                    result.StatusText = "Ваша заявка успешно отправлена на модерацию.";
                }

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
        /// Метод создаст заявку бизнеса.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="businessId">Id бизнеса, по которому оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        public async Task<RequestBusinessOutput> CreateRequestBusinessAsync(string userName, string phone, string account, long businessId)
        {
            try
            {
                var result = await _businessRepository.CreateRequestBusinessAsync(userName, phone, account, businessId);

                if (result != null)
                {
                    result.IsSuccessCreatedRequest = true;
                    result.StatusText = "Ваша заявка успешно отправлена на модерацию.";
                }

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
        /// Метод проверит существование заявок.
        /// </summary>
        /// <param name="id">Id франшизы или бизнеса.</param>
        /// <param name="type">Тип франшиза или бизнес.</param>
        /// <param name="account">Текущий пользователь.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<bool> CheckConfirmedRequestAsync(string id, string type, string account)
        {
            try
            {
                if (Convert.ToInt64(id) <= 0 || string.IsNullOrEmpty(type))
                {
                    throw new EmptyRequestInputParamsException(id, type);
                }

                // Если нужно искать заявки бизнеса.
                if (type.Equals("Business"))
                {
                    // Найдет заявки бизнеса, которые подтверждены продавцом.
                    var isAnyConfirmedRequests = await _businessRepository.CheckBusinessRequestAsync(Convert.ToInt64(id), account);

                    // Если подтвержденных заявок нет, вернет отказ.
                    if (!isAnyConfirmedRequests)
                    {
                        return false;
                    }
                }

                // Если нужно искать заявки франшиз.
                if (type.Equals("Franchise"))
                {
                    // Найдет заявки франшиз, которые подтверждены продавцом.
                    var isAnyConfirmedRequests = await _franchiseRepository.CheckFranchiseRequestAsync(Convert.ToInt64(id), account);

                    // Если подтвержденных заявок нет, вернет отказ.
                    if (!isAnyConfirmedRequests)
                    {
                        return false;
                    }
                }

                return true;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}
