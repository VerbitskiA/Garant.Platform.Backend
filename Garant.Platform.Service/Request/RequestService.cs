using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Entities.Business;
using Garant.Platform.Models.Request.Output;

namespace Garant.Platform.Services.Request
{
    /// <summary>
    /// Класс реализует методы сервиса заявок.
    /// </summary>
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
        /// Метод получит список заявок для вкладки профиля "Уведомления".
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Список заявок.</returns>
        public async Task<IEnumerable<RequestBusinessOutput>> GetBusinessRequestsAsync(string account)
        {
            try
            {
                var requestsList = await _businessRepository.GetBusinessRequestsAsync(account);

                var mapper = AutoFac.Resolve<IMapper>();
                var result = mapper.Map<IEnumerable<RequestBusinessOutput>>(requestsList);

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
