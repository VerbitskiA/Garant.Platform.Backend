using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Request.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Request
{
    /// <summary>
    /// Класс реализует методы репозитория заявок.
    /// </summary>
    public class RequestRepository : IRequestRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IFranchiseRepository _franchiseRepository;
        private readonly IBusinessRepository _businessRepository;

        public RequestRepository(PostgreDbContext postgreDbContext, 
            IUserRepository userRepository,
            IFranchiseRepository franchiseRepository,
            IBusinessRepository businessRepository)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
            _franchiseRepository = franchiseRepository;
            _businessRepository = businessRepository;
        }

        /// <summary>                                                
        /// Метод получит список сделок пользователя.                
        /// </summary>                                               
        /// <param name="account">Аккаунт.</param>                   
        /// <returns>Список сделок.</returns>            
        public async Task<IEnumerable<RequestDealOutput>> GetDealsAsync(string account)
        {
            try
            {
                var result = new List<RequestDealOutput>();
                
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new NotFoundUserIdException(account);
                }
                
                // Найдет ФИО текущего пользователя.
                var currentUserInfo = await _postgreDbContext.UsersInformation
                    .Where(u => u.UserId.Equals(userId))
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.Patronymic
                    })
                    .FirstOrDefaultAsync();

                var currentUserFio = string.Empty;

                if (currentUserInfo != null)
                {
                    currentUserFio = currentUserInfo.FirstName + " "
                        + currentUserInfo.LastName?.Substring(0, 1) + "."
                        + currentUserInfo.Patronymic?.Substring(0, 1);
                }

                // Получит заявки по бизнесам и франшизам.
                var requestsFranchises = await _franchiseRepository.GetFranchiseRequestsAsync(account);
                var requestsBusinesses = await _businessRepository.GetBusinessRequestsAsync(account);

                // Дополнит информацию о заявках бизнеса.
                var requestsFranchisesList = requestsFranchises.ToList();
                
                if (requestsFranchisesList.Any())
                {
                    // Проходит по заявкам франшиз и дополнит их.
                    foreach (var item in requestsFranchisesList)
                    {
                        var franchise = await _franchiseRepository.GetFranchiseAsync(item.FranchiseId, null);
                        
                        // Найдет ФИО владельца.
                        var ownerUserInfo = await _postgreDbContext.UsersInformation
                            .Where(u => u.UserId.Equals(franchise.UserId))
                            .Select(u => new
                            {
                                u.FirstName,
                                u.LastName,
                                u.Patronymic
                            })
                            .FirstOrDefaultAsync();
                        
                        var ownerUserFio = string.Empty;

                        if (ownerUserInfo != null)
                        {
                            ownerUserFio = ownerUserInfo.FirstName + " "
                                           + ownerUserInfo.LastName?.Substring(0, 1) + "."
                                           + ownerUserInfo.Patronymic?.Substring(0, 1);
                        }

                        var requestService = AutoFac.Resolve<IRequestService>();
                        var statusText = await requestService.GetDealRequestInfoAsync(item.RequestStatus);

                        result.Add(new RequestDealOutput
                        {
                            ButtonActionText = "Перейти в сделку",
                            DealItemTitle = franchise.Title,
                            ItemDealIdId = franchise.FranchiseId,
                            ItemId = item.RequestId,
                            Status = item.RequestStatus,
                            MiddleText = "Сделка между",
                            Type = "Franchise",
                            CurrentUserName = currentUserFio,
                            OwnerDeaItemUserName = ownerUserFio,
                            ItemDealUrl = franchise.Url,
                            StatusTitle = "Статус сделки:",
                            StatusText = statusText.Item1,
                        });
                    }
                }

                var requestBusinessList = requestsBusinesses.ToList();
                
                if (requestBusinessList.Any())
                {
                    // Проходит по заявкам бизнеса и дополнит их.
                    foreach (var item in requestBusinessList)
                    {
                        var business = await _businessRepository.GetBusinessAsync(item.BusinessId, null);
                        
                        // Найдет ФИО владельца.
                        var ownerUserInfo = await _postgreDbContext.UsersInformation
                            .Where(u => u.UserId.Equals(business.UserId))
                            .Select(u => new
                            {
                                u.FirstName,
                                u.LastName,
                                u.Patronymic
                            })
                            .FirstOrDefaultAsync();
                        
                        var ownerUserFio = string.Empty;

                        if (ownerUserInfo != null)
                        {
                            ownerUserFio = ownerUserInfo.FirstName + " "
                                           + ownerUserInfo.LastName?.Substring(0, 1) + "."
                                           + ownerUserInfo.Patronymic?.Substring(0, 1);
                        }
                        
                        var requestService = AutoFac.Resolve<IRequestService>();
                        var statusText = await requestService.GetDealRequestInfoAsync(item.RequestStatus);

                        result.Add(new RequestDealOutput
                        {
                            ButtonActionText = "Перейти в сделку",
                            DealItemTitle = business.BusinessName,
                            ItemDealIdId = business.BusinessId,
                            ItemId = item.RequestId,
                            Status = item.RequestStatus,
                            MiddleText = "Сделка между",
                            Type = "Business",
                            CurrentUserName = currentUserFio,
                            OwnerDeaItemUserName = ownerUserFio,
                            ItemDealUrl = business.Url,
                            StatusText = statusText.Item1,
                            StatusTitle = "Статус сделки:",
                        });
                    }
                }

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
        /// етод проверит подтверждена ли заявка продавцом.
        /// </summary>
        /// <param name="requestId">Id аявки.</param>
        /// <param name="type">Тип заявки.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<bool> CheckConfirmRequestAsync(long requestId, string type)
        {
            try
            {
                // Если бизнес.
                if (type.Equals("Business"))
                {
                    var checkBusinessRequest = await _postgreDbContext.RequestsBusinesses
                        .Where(b => b.RequestId == requestId && b.RequestStatus.Equals("Confirmed"))
                        .FirstOrDefaultAsync();

                    if (checkBusinessRequest == null)
                    {
                        return false;
                    }
                }
                
                // Если франшиза.
                if (type.Equals("Franchise"))
                {
                    var checkFranchiseRequest = await _postgreDbContext.RequestsFranchises
                        .Where(b => b.RequestId == requestId && b.RequestStatus.Equals("Confirmed"))
                        .FirstOrDefaultAsync();

                    if (checkFranchiseRequest == null)
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