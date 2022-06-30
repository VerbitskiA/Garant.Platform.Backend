using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Consts;
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
        private readonly INotificationsService _notificationsService;

        public RequestService(IFranchiseRepository franchiseRepository, 
            IBusinessRepository businessRepository,
            INotificationsService notificationsService)
        {
            _franchiseRepository = franchiseRepository;
            _businessRepository = businessRepository;
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
            _notificationsService = notificationsService;
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
        public async Task<RequestFranchiseOutput> CreateRequestFranchiseAsync(string userName, string phone,
            string city, string account, long franchiseId)
        {
            try
            {
                RequestFranchiseOutput result = null;
                var userRepository = AutoFac.Resolve<IUserRepository>();
                var userId = await userRepository.FindUserIdUniverseAsync(account);
                var userInfo = await userRepository.GetUserProfileInfoByIdAsync(userId);
                
                if (userInfo == null)
                {
                    throw new NotFoundUserInfoException(account);
                }
                
                // Проверит, заполнил ли пользователь все обязательные данные в профиле. 
                                // Если не заполнены, то выдаст сообщение SignalR.
                                if (string.IsNullOrEmpty(userInfo.FirstName) 
                                    || string.IsNullOrEmpty(userInfo.LastName)
                                    || string.IsNullOrEmpty(userInfo.Email)
                                    || string.IsNullOrEmpty(userInfo.PhoneNumber)
                                    || string.IsNullOrEmpty(userInfo.Inn)
                                    || string.IsNullOrEmpty(userInfo.Kpp)
                                    || string.IsNullOrEmpty(userInfo.Bik)
                                    || string.IsNullOrEmpty(userInfo.Kpp)
                                    || string.IsNullOrEmpty(userInfo.CorrAccountNumber)
                                    || string.IsNullOrEmpty(userInfo.DefaultBankName)
                                    || (userInfo.PassportSerial == null)
                                    || userInfo.PassportNumber == null
                                    || string.IsNullOrEmpty(userInfo.DateGive)
                                    || string.IsNullOrEmpty(userInfo.WhoGive)
                                    || string.IsNullOrEmpty(userInfo.DateGive)
                                    || string.IsNullOrEmpty(userInfo.Code)
                                    || string.IsNullOrEmpty(userInfo.AddressRegister))
                                {
                                    await _notificationsService.SendNotifyEmptyUserInfoAsync();
                                    
                                    return new RequestFranchiseOutput
                                    {
                                        IsSuccessCreatedRequest = false,
                                        RequestStatus = NotifyMessage.NOTIFY_EMPTY_USER_INFO
                                    };
                                }
                
                result = await _franchiseRepository.CreateRequestFranchiseAsync(userName, phone, city, account, franchiseId);

                if (result != null)
                {
                    result.IsSuccessCreatedRequest = true;
                    result.RequestStatus = NotifyMessage.REQUEST_MODERATION;
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
        /// Метод создаст заявку бизнеса.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="businessId">Id бизнеса, по которому оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        public async Task<RequestBusinessOutput> CreateRequestBusinessAsync(string userName, string phone, string email,
            string account, long businessId)
        {
            try
            {
                RequestBusinessOutput result = null;
                var userRepository = AutoFac.Resolve<IUserRepository>();
                var userId = await userRepository.FindUserIdUniverseAsync(account);
                var userInfo = await userRepository.GetUserProfileInfoByIdAsync(userId);
                
                if (userInfo == null)
                {
                    throw new NotFoundUserInfoException(account);
                }

                #region Если пользователь неавторизован, то не надо смотреть его профиль
                // Проверит, заполнил ли пользователь все обязательные данные в профиле. 
                // Если не заполнены, то выдаст сообщение SignalR.
                //if (string.IsNullOrEmpty(userInfo.FirstName) 
                //    || string.IsNullOrEmpty(userInfo.LastName)
                //    || string.IsNullOrEmpty(userInfo.Email)
                //    || string.IsNullOrEmpty(userInfo.PhoneNumber)
                //    || string.IsNullOrEmpty(userInfo.Inn)
                //    || string.IsNullOrEmpty(userInfo.Kpp)
                //    || string.IsNullOrEmpty(userInfo.Bik)
                //    || string.IsNullOrEmpty(userInfo.Kpp)
                //    || string.IsNullOrEmpty(userInfo.CorrAccountNumber)
                //    || string.IsNullOrEmpty(userInfo.DefaultBankName)
                //    || userInfo.PassportSerial == null
                //    || userInfo.PassportNumber == null
                //    || string.IsNullOrEmpty(userInfo.DateGive)
                //    || string.IsNullOrEmpty(userInfo.WhoGive)
                //    || string.IsNullOrEmpty(userInfo.DateGive)
                //    || string.IsNullOrEmpty(userInfo.Code)
                //    || string.IsNullOrEmpty(userInfo.AddressRegister))
                //{
                //    await _notificationsService.SendNotifyEmptyUserInfoAsync();
                    
                //    return new RequestBusinessOutput
                //    {
                //        IsSuccessCreatedRequest = false,
                //        StatusText = NotifyMessage.NOTIFY_EMPTY_USER_INFO
                //    };
                //}
                #endregion

                result = await _businessRepository.CreateRequestBusinessAsync(userName, phone, email, account, businessId);
                
                if (result != null)
                {
                    result.IsSuccessCreatedRequest = true;
                    result.StatusText = NotifyMessage.REQUEST_MODERATION;

                    var mailingService = AutoFac.Resolve<IMailingService>();

                    var business = await _businessRepository.GetBusinessAsync(businessId, "View");

                    var userDestination = await userRepository.FindUserById(business.UserId);

                    string title = "На ваш бизнес поступила заявка!";

                    string message = $"На ваш бизнес {business.BusinessName} поступила заявка от пользователя {userName}. С ним можно связаться по телефону {phone} или написать на {email}";

                   await  mailingService.SendAcceptEmailAsync(userDestination.Email, message, title);
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
        /// Метод получит список заявок для вкладки профиля "Уведомления".
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Список заявок.</returns>
        public async Task<IEnumerable<RequestOutput>> GetUserRequestsAsync(string account)
        {
            try
            {
                IEnumerable<RequestBusinessOutput> businessRequests = null;
                IEnumerable<RequestFranchiseOutput> franchiseRequests = null;
                var result = new List<RequestOutput>();

                // Получит список заявок по бизнесам.
                var requestsBusinessList = await _businessRepository.GetBusinessRequestsAsync(account);

                var mapper = AutoFac.Resolve<IMapper>();

                // Если есть заявки по бизнесам.
                if (requestsBusinessList.Any())
                {
                    businessRequests = mapper.Map<IEnumerable<RequestBusinessOutput>>(requestsBusinessList);
                }

                // Получит список заявок по франшизам.
                var requestsFranchiseList = await _franchiseRepository.GetFranchiseRequestsAsync(account);

                // Если есть заявки по франшизам.
                if (requestsFranchiseList.Any())
                {
                    franchiseRequests = mapper.Map<IEnumerable<RequestFranchiseOutput>>(requestsFranchiseList);
                }

                // Добавит бизнесы к результату.
                if (businessRequests != null)
                {
                    foreach (var b in businessRequests)
                    {
                        var request = new RequestOutput
                        {
                            RequestId = b.RequestId,
                            RequestItemId = b.BusinessId,
                            UserId = b.UserId,
                            RequestType = "Business",
                            RequestStatus = b.RequestStatus
                        };
                        
                        var status = await GetRequestInfoAsync(b.RequestStatus);

                        request.NotifyTitle = status.Item1;
                        request.NotifyDescription = status.Item2;
                        
                        result.Add(request);
                    }
                }

                // Добавит франшизы к результату.
                if (franchiseRequests != null)
                {
                    foreach (var f in franchiseRequests)
                    {
                        var request = new RequestOutput
                        {
                            RequestId = f.RequestId,
                            RequestItemId = f.FranchiseId,
                            UserId = f.UserId,
                            RequestType = "Franchise"
                        };

                        var status = await GetRequestInfoAsync(f.RequestStatus);

                        request.NotifyTitle = status.Item1;
                        request.NotifyDescription = status.Item2;

                        result.Add(request);
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
        /// Метод вернет заголовок и описание статуса заявки исходя из ее статуса.
        /// </summary>
        /// <param name="requestStatus">Статус заявки.&</param>
        /// <returns>Заголовок и описание статуса заявки.</returns>
        private async Task<(string, string)> GetRequestInfoAsync(string requestStatus) {
            var result = (string.Empty, string.Empty);
            
            // Если на рассмотрении.
            if (requestStatus.Equals("Review"))
            {
                result.Item1 = NotifyMessage.REQUEST_REVIEW;
                result.Item2 = NotifyMessage.REQUEST_REVIEW_DETAIL;
            }

            return await Task.FromResult(result);
        }
        
        /// <summary>
        /// Метод отправит заявку с посадочных страниц на почту сервиса.
        /// </summary>
        /// <param name="name">Имя пользователя, который оставляет заявку.</param>
        /// <param name="phoneNumber">Телефон пользователя, который оставляет заявку.</param>
        /// <param name="landingType">Тип посадочной страницы.</param>
        public async Task SendLandingRequestAsync(string name, string phoneNumber, string landingType)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber))
                {
                    throw new EmptyRequestLandingParamsException();
                }
                
                // Отправит заявку на почту администрации сервиса.
                var mailingService = AutoFac.Resolve<IMailingService>();
                await mailingService.SendMailLandingReuestAsync(name, phoneNumber, landingType);
                
                // Отправит уведомление на фронт.
                await _notificationsService.SendLandingRequestMessageAsync();
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