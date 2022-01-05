using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Messaging.Abstraction.Chat;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Service.Garant
{
    /// <summary>
    /// Класс реализует методы сервиса Гаранта, где определяются различные действия.
    /// </summary>
    public sealed class GarantActionService : IGarantActionService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IFranchiseService _franchiseService;
        private readonly IChatService _chatService;
        private readonly IBusinessService _businessService;
        private readonly IGarantActionRepository _garantActionRepository;

        public GarantActionService(PostgreDbContext postgreDbContext, IUserRepository userRepository, IFranchiseService franchiseService, IChatService chatService, IBusinessService businessService, IGarantActionRepository garantActionRepository)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
            _franchiseService = franchiseService;
            _chatService = chatService;
            _businessService = businessService;
            _garantActionRepository = garantActionRepository;
        }

        /// <summary>
        /// Метод получит данные для стартовой страницы в Гаранте.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса, с которым зашли в Гарант.</param>
        /// <param name="orderType">Тип заказа франшиза или бизнес.</param>
        /// <param name="account">Аккаунт.</param>
        /// <param name="stage">Номер этапа.</param>
        /// <param name="isChat">Флаг чата.</param>
        /// <param name="otherId">Id другого пользователя.</param>
        /// <returns>Данные стартовой страницы.</returns>
        public async Task<InitGarantDataOutput> GetInitDataGarantAsync(long originalId, string orderType, string account, int stage, bool isChat, string otherId)
        {
            try
            {
                var result = new InitGarantDataOutput();
                var currentUserId = await _userRepository.FindUserIdUniverseAsync(account);
                var userName = await _userRepository.GetUserProfileInfoByIdAsync(currentUserId);

                // Найдет Id владельца предмета обсуждения (т.е франшизы или бизнеса).
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Если заявка франшизы, то сравнит Id с владельцем франшизы.
                if (orderType.Equals("Franchise"))
                {
                    var franchise = await _franchiseService.GetFranchiseAsync(originalId);

                    if (franchise != null)
                    {
                        var iterationList = JsonConvert.DeserializeObject<List<ConvertInvestIncludePriceOutput>>(franchise.InvestInclude);

                        // Получит Id сделки.
                        var dateCreateDeal = await _garantActionRepository.GetOwnerIdItemDealAsync(franchise.UserId);

                        // Сравнит Id.
                        var isOwner = userId.Equals(franchise.UserId);

                        // Если нужно подтянуть данные не владельца предмета сделки.
                        if (!isOwner && stage == 1)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise1IterationsNotOwnerList = GetDataFranchise1IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, dateCreateDeal);

                            return franchise1IterationsNotOwnerList;
                        }

                        if (isOwner && stage == 1)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var franchise1IterationsOwnerList = GetDataFranchise1IterationOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, dateCreateDeal);

                            return franchise1IterationsOwnerList;
                        }

                        // Если этап 2 и не владелец.
                        if (!isOwner && stage == 2)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise2IterationsNotOwnerList = await GetDataFranchise2IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, franchise.InvestInclude, iterationList, isChat, account, dateCreateDeal);

                            return franchise2IterationsNotOwnerList;
                        }

                        // Если этап 2 и владелец.
                        if (isOwner && stage == 2)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var franchise2IterationsOwnerList = await GetDataFranchise2IterationOwner(franchise.FranchiseId,
                                franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName,
                                userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId,
                                franchise.InvestInclude, iterationList, isChat, account, dateCreateDeal);

                            return franchise2IterationsOwnerList;
                        }

                        // Если этап 3 и не владелец.
                        if (!isOwner && stage == 3)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise3IterationsNotOwnerList = await GetDataFranchise3IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, franchise.InvestInclude, iterationList, isChat, account, dateCreateDeal);

                            return franchise3IterationsNotOwnerList;
                        }

                        // Если этап 3 и владелец.
                        if (isOwner && stage == 3)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var franchise3IterationsOwnerList = await GetDataFranchise3IterationOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, franchise.InvestInclude, iterationList, isChat, account, dateCreateDeal);

                            return franchise3IterationsOwnerList;
                        }

                        // Если этап 4 и не владелец.
                        if (!isOwner && stage == 4)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise4IterationsNotOwnerList = await GetDataFranchise4IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, franchise.InvestInclude, iterationList, isChat, account, dateCreateDeal);

                            return franchise4IterationsNotOwnerList;
                        }

                        // Если этап 4 и владелец.
                        if (isOwner && stage == 4)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var franchise4IterationsOwnerList = await GetDataFranchise4IterationOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, franchise.InvestInclude, iterationList, isChat, account, dateCreateDeal);

                            return franchise4IterationsOwnerList;
                        }
                    }
                }

                // Если заявка бизнеса, то сравнит Id с владельцем бизнеса.
                if (orderType.Equals("Business"))
                {
                    var business = await _businessService.GetBusinessAsync(originalId);

                    if (business != null)
                    {
                        var iterationList = JsonConvert.DeserializeObject<List<ConvertInvestIncludePriceOutput>>(business.InvestPrice);

                        var dateCreateDeal = await _garantActionRepository.GetOwnerIdItemDealAsync(business.UserId);

                        // Сравнит Id.
                        var isOwner = userId.Equals(business.UserId);

                        // Если нужно подтянуть данные не владельца предмета сделки.
                        if (!isOwner && stage == 1)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(business.UserId);

                            var business1IterationsNotOwnerList = GetDataBusiness1IterationNotOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, dateCreateDeal);

                            return business1IterationsNotOwnerList;
                        }

                        if (isOwner && stage == 1)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsBusinesses
                                .Where(f => f.BusinessId == business.BusinessId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var business1IterationsOwnerList = GetDataBusiness1IterationOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, dateCreateDeal);

                            return business1IterationsOwnerList;
                        }

                        // Если этап 2 и не владелец.
                        if (!isOwner && stage == 2)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(business.UserId);

                            var business2IterationsNotOwnerList = await GetDataBusiness2IterationNotOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, business.InvestPrice, iterationList, isChat, account, dateCreateDeal);

                            return business2IterationsNotOwnerList;
                        }

                        // Если этап 2 и владелец.
                        if (isOwner && stage == 2)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsBusinesses
                                .Where(f => f.BusinessId == business.BusinessId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var business2IterationsOwnerList = await GetDataBusiness2IterationOwner(business.BusinessId,
                                business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName,
                                userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId,
                                business.InvestPrice, iterationList, isChat, account, dateCreateDeal);

                            return business2IterationsOwnerList;
                        }

                        // Если этап 3 и не владелец.
                        if (!isOwner && stage == 3)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(business.UserId);

                            var business3IterationsNotOwnerList = await GetDataBusiness3IterationNotOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, business.InvestPrice, iterationList, isChat, account, dateCreateDeal);

                            return business3IterationsNotOwnerList;
                        }

                        // Если этап 3 и владелец.
                        if (isOwner && stage == 3)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsBusinesses
                                .Where(f => f.BusinessId == business.BusinessId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var business3IterationsOwnerList = await GetDataBusiness3IterationOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, business.InvestPrice, iterationList, isChat, account, dateCreateDeal);

                            return business3IterationsOwnerList;
                        }

                        // Если этап 4 и не владелец.
                        if (!isOwner && stage == 4)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(business.UserId);

                            var business4IterationsNotOwnerList = await GetDataBusiness4IterationNotOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, business.InvestPrice, iterationList, isChat, account, dateCreateDeal);

                            return business4IterationsNotOwnerList;
                        }

                        // Если этап 4 и владелец.
                        if (isOwner && stage == 4)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsBusinesses
                                .Where(f => f.BusinessId == business.BusinessId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var business4IterationsOwnerList = await GetDataBusiness4IterationOwner(business.BusinessId, business.TotalInvest, business.BusinessName, business.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, business.InvestPrice, iterationList, isChat, account, dateCreateDeal);

                            return business4IterationsOwnerList;
                        }
                    }
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
        /// Метод формирует данные франшизы для 1 этапа не для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private InitGarantDataOutput GetDataFranchise1IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = string.Format("{0:0,0}", totalInvest),
                BlackBlockText = @"Как доверить посреднику большую сумму? Холдирование – это временная блокировка (замораживание) определенной суммы на банковском счете.
Осуществляется банком-эмитентом в момент авторизации на срок, в течение которого проводит расчеты банк-эквайер. Деньги не поступают
на счет нашего юридического лица, а удерживаются банком / эквайрингом “Название”.

Чекбокс приравнивается к подписанному договору простой электронной подписью, однако вы можете запросить подписанные документы
с нашей стороны о намерениях или о соглашении.",
                BlackBlockTitle = "Мои гарантии и что такое холдирование?",
                BlackBlueButtonText = "Запросить договор от GoBizy",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Продавец и сервис-посредник (gobizy) должны быть застрахованы от мошенничества. Как только вы подтвердите наличие необходимой суммы 
деньги будут холдированы и списываться по мере закрытия каждого этапа. Сумма резервируется и будет возвращена при отмене сделки с учетом
комиссии за использование сервиса в ?%.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Подтверждение покупательской способности",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию этапов",
                ButtonActionText = string.Empty,
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            return result;
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 1 этапа не для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private InitGarantDataOutput GetDataBusiness1IterationNotOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = string.Format("{0:0,0}", totalInvest),
                BlackBlockText = @"Как доверить посреднику большую сумму? Холдирование – это временная блокировка (замораживание) определенной суммы на банковском счете.
Осуществляется банком-эмитентом в момент авторизации на срок, в течение которого проводит расчеты банк-эквайер. Деньги не поступают
на счет нашего юридического лица, а удерживаются банком / эквайрингом “Название”.

Чекбокс приравнивается к подписанному договору простой электронной подписью, однако вы можете запросить подписанные документы
с нашей стороны о намерениях или о соглашении.",
                BlackBlockTitle = "Мои гарантии и что такое холдирование?",
                BlackBlueButtonText = "Запросить договор от GoBizy",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Продавец и сервис-посредник (gobizy) должны быть застрахованы от мошенничества. Как только вы подтвердите наличие необходимой суммы 
деньги будут холдированы и списываться по мере закрытия каждого этапа. Сумма резервируется и будет возвращена при отмене сделки с учетом
комиссии за использование сервиса в ?%.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Подтверждение покупательской способности",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию этапов",
                ButtonActionText = string.Empty,
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            return result;
        }

        /// <summary>
        /// Метод формирует данные франшизы для 1 этапа для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private InitGarantDataOutput GetDataFranchise1IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, DateTime dateCreateDeal)
        {
            // Если нужно подтянуть данные владельца предмета сделки.
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Покупатель холдировал средства и теперь необходима помощь в создании договора? Юрист сервиса GoBizy готов помочь в составлении
основного договора по продаже бизнеса и составления актов приема-передачи.",
                BlackBlockTitle = "Помощь юриста",
                BlackBlueButtonText = "Пригласить юриста в сделку",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Покупатель подтвердил свою платежеспособность и холдировал (сервис удержал сумму как посредник) сумму указанную в карточке вашего готового бизнеса. Таким образом стороны застрахованы от неплатежеспособных приобретателей. Теперь вам нужно подтвердить продажу, если это еще актуально.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Подтверждение покупательской способности",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию этапов",
                ButtonActionText = $"Подтвердить продажу на {totalInvest} ₽",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                Role = "Продавец (Вы)",
                OtherUserRole = "Покупатель",
                ButtonCancel = "Отменить",
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                BlockDocumentDealName = "Документы сделки",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            return result;
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 1 этапа для владельца.
        /// </summary>
        /// <param name="businessId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private InitGarantDataOutput GetDataBusiness1IterationOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, DateTime dateCreateDeal)
        {
            // Если нужно подтянуть данные владельца предмета сделки.
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Покупатель холдировал средства и теперь необходима помощь в создании договора? Юрист сервиса GoBizy готов помочь в составлении
основного договора по продаже бизнеса и составления актов приема-передачи.",
                BlackBlockTitle = "Помощь юриста",
                BlackBlueButtonText = "Пригласить юриста в сделку",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Покупатель подтвердил свою платежеспособность и холдировал (сервис удержал сумму как посредник) сумму указанную в карточке вашего готового бизнеса. Таким образом стороны застрахованы от неплатежеспособных приобретателей. Теперь вам нужно подтвердить продажу, если это еще актуально.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Подтверждение покупательской способности",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию этапов",
                ButtonActionText = $"Подтвердить продажу на {totalInvest} ₽",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                Role = "Продавец (Вы)",
                OtherUserRole = "Покупатель",
                ButtonCancel = "Отменить",
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                BlockDocumentDealName = "Документы сделки",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            return result;
        }

        /// <summary>
        /// Метод формирует данные франшизы для 2 этапа не для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise2IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.
Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlackBlockTitle = "Сделка спланирована автоматически",
                BlackBlueButtonText = "Запросить договор от GoBizy",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Продавец и сервис-посредник (gobizy) должны быть застрахованы от мошенничества. Как только вы подтвердите наличие необходимой суммы 
деньги будут холдированы и списываться по мере закрытия каждого этапа. Сумма резервируется и будет возвращена при отмене сделки с учетом
комиссии за использование сервиса в ?%.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Планирование сделки",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию договора",
                ButtonActionText = "Принять",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 2 этапа не для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataBusiness2IterationNotOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.
Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlackBlockTitle = "Сделка спланирована автоматически",
                BlackBlueButtonText = "Запросить договор от GoBizy",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Продавец и сервис-посредник (gobizy) должны быть застрахованы от мошенничества. Как только вы подтвердите наличие необходимой суммы 
деньги будут холдированы и списываться по мере закрытия каждого этапа. Сумма резервируется и будет возвращена при отмене сделки с учетом
комиссии за использование сервиса в ?%.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Планирование сделки",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию договора",
                ButtonActionText = "Принять",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные франшизы для 2 этапа для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise2IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            // Если нужно подтянуть данные владельца предмета сделки.
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Покупатель холдировал средства и теперь необходима помощь в создании договора? Юрист сервиса GoBizy готов помочь в составлении
основного договора по продаже бизнеса и составления актов приема-передачи.",
                BlackBlockTitle = "Сделка спланирована автоматически",
                BlackBlueButtonText = "Пригласить юриста в сделку",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.

Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Подтверждение продажи",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию договора",
                ButtonActionText = "Отправить покупателю",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                Role = "Продавец (Вы)",
                OtherUserRole = "Покупатель",
                ButtonCancel = "Изменить",
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                BlockDocumentDealName = "Документы сделки",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 2 этапа для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataBusiness2IterationOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            // Если нужно подтянуть данные владельца предмета сделки.
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Покупатель холдировал средства и теперь необходима помощь в создании договора? Юрист сервиса GoBizy готов помочь в составлении
основного договора по продаже бизнеса и составления актов приема-передачи.",
                BlackBlockTitle = "Сделка спланирована автоматически",
                BlackBlueButtonText = "Пригласить юриста в сделку",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.

Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlockRightStatusTitle = "Холдирование средств",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Подтверждение продажи",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к согласованию договора",
                ButtonActionText = "Отправить покупателю",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                Role = "Продавец (Вы)",
                OtherUserRole = "Покупатель",
                ButtonCancel = "Изменить",
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                BlockDocumentDealName = "Документы сделки",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные франшизы для 3 этапа не для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise3IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.
Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlackBlockTitle = "Сделка спланирована автоматически",
                BlackBlueButtonText = "Пригласить в сделку юриста",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.

Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlockRightStatusTitle = "Основной договор",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Согласование договора",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Оплатить первый этап и получению этапов",
                ButtonActionText = "Договор утвержден",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Основной договор",
                ContractDetail = "Договор на проверку от продавца",
                ButtonActionTextContract = "Утвердить договор",
                ButtonRejectDocumentText = "Отклонить договор",
                ContractText = @"Скачайте и проверьте договор присланный продавцом",
                BlockCustomerComment = "Прикрепите скан согласованного договора",
                ButtonApproveDocumentText = "Отправить подписанный договор продавцу в ответ",
                IsOwner = false,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 3 этапа не для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataBusiness3IterationNotOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.
Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlackBlockTitle = "Сделка спланирована автоматически",
                BlackBlueButtonText = "Пригласить в сделку юриста",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.

Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlockRightStatusTitle = "Основной договор",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Согласование договора",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Оплатить первый этап и получению этапов",
                ButtonActionText = "Договор утвержден",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Основной договор",
                ContractDetail = "Договор на проверку от продавца",
                ButtonActionTextContract = "Утвердить договор",
                ButtonRejectDocumentText = "Отклонить договор",
                ContractText = @"Скачайте и проверьте договор присланный продавцом",
                BlockCustomerComment = "Прикрепите скан согласованного договора",
                ButtonApproveDocumentText = "Отправить подписанный договор продавцу в ответ",
                IsOwner = false,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные франшизы для 3 этапа для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise3IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.
Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlackBlockTitle = "Продавец составляет и отправляет договор",
                BlackBlueButtonText = "Запросить договор от GoBizy",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"По правилам нашего сервиса продавец сам или посредством привлечения своих юристов составляет и отправляет договор на согласование 
покупателю. Если у вас нет собственных юристов или опыта в создании договора - пригласите юриста со стороны нашего сервиса.",
                BlockRightStatusTitle = "Договор от покупателя",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Согласование договора",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к исполнению этапов",
                ButtonActionText = "Договор на согласовании",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Покупатель",
                Role = "Продавец (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Основной договор",
                ContractDetail = "Скан согласованного договора",
                ButtonActionTextContract = "Отправить на согласование покупателю",
                ContractText = @"Прикрепите подписанный вашей стороной файл в формате .pdf и ожидайте подтверждения от продавца. После утверждения договора вы получите оплату
 за первый этап сделки.",
                BlockCustomerComment = "Вы получите договор в ответ после отправки и утверждения со стороны покупателя.",
                ButtonApproveDocumentText = "Согласен с договором от покупателя",
                ButtonRejectDocumentText = "Отклонить договор",
                IsOwner = true,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 3 этапа для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataBusiness3IterationOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = @"Этапы сделки планируются автоматически - прикрепляются из карточки бизнеса, а именно из раздела “Входит в стоимость бизнеса”. 
Однако, если что-то вас не устраивает вы можете согласовать иные этапы с продавцом - воспользуйтесь чатом для связи с продавцом.
Если вы не уверены в собственных юридических знаниях, то можете пригласить в сделку юриста от нашего сервиса для помощи с составлением
договора.",
                BlackBlockTitle = "Продавец составляет и отправляет договор",
                BlackBlueButtonText = "Запросить договор от GoBizy",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = @"По правилам нашего сервиса продавец сам или посредством привлечения своих юристов составляет и отправляет договор на согласование 
покупателю. Если у вас нет собственных юристов или опыта в создании договора - пригласите юриста со стороны нашего сервиса.",
                BlockRightStatusTitle = "Договор от покупателя",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Согласование договора",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Перейти к исполнению этапов",
                ButtonActionText = "Договор на согласовании",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Покупатель",
                Role = "Продавец (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Основной договор",
                ContractDetail = "Скан согласованного договора",
                ButtonActionTextContract = "Отправить на согласование покупателю",
                ContractText = @"Прикрепите подписанный вашей стороной файл в формате .pdf и ожидайте подтверждения от продавца. После утверждения договора вы получите оплату
 за первый этап сделки.",
                BlockCustomerComment = "Вы получите договор в ответ после отправки и утверждения со стороны покупателя.",
                ButtonApproveDocumentText = "Согласен с договором от покупателя",
                ButtonRejectDocumentText = "Отклонить договор",
                IsOwner = true,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }

        /// <summary>
        /// Метод формирует данные франшизы для 4 этапа для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise4IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = string.Empty,
                BlackBlockTitle = "Оплата и исполнение этапов сделки",
                BlackBlueButtonText = "Отправить акт на согласование покупателю",
                BlackButtonText = string.Empty,
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = string.Empty,
                BlockRightStatusTitle = string.Empty,
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Получение оплаты и исполнение этапов сделки",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Завершить сделку",
                ButtonActionText = "Утвердить акт",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Скан акта приема-передачи",
                ContractDetail = "После оплаты акта вы сможете утвердить его посредством подписанного акта приема-передачи",
                ButtonActionTextContract = "Оплатить этап - " + string.Format("{0:0,0}", totalInvest),
                ButtonRejectDocumentText = string.Empty,
                ContractText = @"Прикрепите согласованный файл в формате .pdf и ожидайте подтверждения от продавца",
                BlockCustomerComment = "После утверждения акта вы сможете оплатить следующий",
                ButtonApproveDocumentText = string.Empty,
                IsOwner = false,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 4 этапа для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataBusiness4IterationOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = string.Empty,
                BlackBlockTitle = "Оплата и исполнение этапов сделки",
                BlackBlueButtonText = "",
                BlackButtonText = string.Empty,
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = string.Empty,
                BlockRightStatusTitle = string.Empty,
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Получение оплаты и исполнение этапов сделки",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Завершить сделку",
                ButtonActionText = string.Empty,
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Покупатель",
                Role = "Продавец (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Скан акта приема-передачи",
                ContractDetail = "Договор на проверку от продавца",
                ButtonActionTextContract = string.Empty,
                ButtonRejectDocumentText = string.Empty,
                ContractText = @"Прикрепите согласованный файл в формате .pdf и ожидайте подтверждения от продавца",
                BlockCustomerComment = "Прикрепите скан согласованного договора",
                ButtonApproveDocumentText = string.Empty,
                IsOwner = true,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Метод формирует данные франшизы для 4 этапа для владельца.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise4IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = franchiseId,
                TotalAmount = totalInvest,
                BlackBlockText = string.Empty,
                BlackBlockTitle = "Оплата и исполнение этапов сделки",
                BlackBlueButtonText = "Пригласить в сделку юриста",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = string.Empty,
                BlockRightStatusTitle = "Скан акта приема-передачи",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Получение оплаты и исполнение этапов сделки",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Завершить сделку",
                ButtonActionText = "Отправить акт на согласование покупателю",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Покупатель",
                Role = "Продавец (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Основной договор",
                ContractDetail = "После утверждения акта вы сможете оплатить следующий",
                ButtonActionTextContract = "Утвердить договор покупателя",
                ButtonRejectDocumentText = "Отклонить договор",
                ContractText = string.Empty,
                BlockCustomerComment = "Прикрепите скан согласованного договора",
                ButtonApproveDocumentText = string.Empty,
                IsOwner = true,
                IsLast = true,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Метод формирует данные бизнеса для 4 этапа не для владельца.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="totalInvest">Всего инвестиций.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="url">Путь.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="otherFirstName">Имя другого пользователя.</param>
        /// <param name="otherLastName">Фамилия другого пользователя.</param>
        /// <param name="orderType">Тип предмета обсуждения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="dateCreateDeal">Дата создания сделки.</param>
        /// <returns>Данные бизнеса.</returns>
        private async Task<InitGarantDataOutput> GetDataBusiness4IterationNotOwner(long businessId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account, DateTime dateCreateDeal)
        {
            var result = new InitGarantDataOutput
            {
                ItemDealId = businessId,
                TotalAmount = totalInvest,
                BlackBlockText = string.Empty,
                BlackBlockTitle = "Оплата и исполнение этапов сделки",
                BlackBlueButtonText = "Пригласить в сделку юриста",
                BlackButtonText = "Не сейчас",
                BlockLeftTitle = "Покупка бизнеса онлайн",
                BlockLeftSumTitle = "На общую сумму",
                BlockRightStatusText = string.Empty,
                BlockRightStatusTitle = "Основной договор",
                BlockRightSumTitle = "Общая сумма",
                BlockRightTitle = "Оплата и подтверждение этапов сделки",
                DocumentBlockTitle = "Документы сделки",
                MainItemTitle = "Предмет сделки",
                ItemTitle = title,
                ContinueButtonText = "Завершить сделку",
                ButtonActionText = "Входит в этап",
                ImageUrl = url,
                Amount = Convert.ToDouble(totalInvest),
                OtherUserRole = "Продавец",
                Role = "Покупатель (Вы)",
                FullName = firstName + " " + lastName,
                OtherUserFullName = otherFirstName + " " + otherLastName,
                ItemDealType = orderType,
                OtherId = userId,
                InvestInclude = investInclude,
                IterationList = iterationList,
                BlockDocumentsTemplatesName = "Шаблоны документов",
                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                ContractTitle = "Основной договор",
                ContractDetail = "",
                ButtonActionTextContract = "Оплатить этап - ",
                ButtonRejectDocumentText = "Отклонить договор",
                ContractText = @"После оплаты акта вы сможете утвердить его посредством подписанного акта приема-передачи",
                BlockCustomerComment = "Прикрепите скан согласованного договора",
                ButtonApproveDocumentText = "Отправить подписанный договор продавцу в ответ",
                IsOwner = false,
                IsLast = true,
                DateStartDeal = dateCreateDeal.ToString("dd.MM.yyyy")
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return await Task.FromResult(result);
        }
    }
}
