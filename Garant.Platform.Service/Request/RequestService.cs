﻿using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Core.Data;
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
