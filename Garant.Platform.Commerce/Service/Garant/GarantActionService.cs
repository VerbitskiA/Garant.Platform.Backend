using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
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

        public GarantActionService(PostgreDbContext postgreDbContext, IUserRepository userRepository, IFranchiseService franchiseService, IChatService chatService)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
            _franchiseService = franchiseService;
            _chatService = chatService;
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

                        // Сравнит Id.
                        var isOwner = userId.Equals(franchise.UserId);

                        // Если нужно подтянуть данные не владельца предмета сделки.
                        if (!isOwner && stage == 1)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise1IterationsNotOwnerList = GetDataFranchise1IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId);

                            return franchise1IterationsNotOwnerList;
                        }

                        else if (isOwner && stage == 1)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var franchise1IterationsOwnerList = GetDataFranchise1IterationOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId);

                            return franchise1IterationsOwnerList;
                        }

                        // Если этап 2 и не владелец.
                        else if (!isOwner && stage == 2)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise2IterationsNotOwnerList = await GetDataFranchise2IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, franchise.InvestInclude, iterationList, isChat, account);

                            return franchise2IterationsNotOwnerList;
                        }

                        // Если этап 2 и владелец.
                        else if (isOwner && stage == 2)
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
                                franchise.InvestInclude, iterationList, isChat, account);

                            return franchise2IterationsOwnerList;
                        }

                        // Если этап 3 и не владелец.
                        if (!isOwner && stage == 3)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            var franchise3IterationsNotOwnerList = await GetDataFranchise3IterationNotOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, userId, franchise.InvestInclude, iterationList, isChat, account);

                            return franchise3IterationsNotOwnerList;
                        }

                        // Если этап 3 и владелец.
                        else if (isOwner && stage == 3)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            var franchise3IterationsOwnerList = await GetDataFranchise3IterationOwner(franchise.FranchiseId, franchise.TotalInvest, franchise.Title, franchise.Url.Split(",")[0], userName.FirstName, userName.LastName, otherAccount.FirstName, otherAccount.LastName, orderType, otherUserId, franchise.InvestInclude, iterationList, isChat, account);

                            return franchise3IterationsOwnerList;
                        }
                    }
                }

                // Если заявка бизнеса, то сравнит Id с владельцем бизнеса.
                if (orderType.Equals("Business"))
                {
                    var franchise = await _franchiseService.GetFranchiseAsync(originalId);

                    if (franchise != null)
                    {
                        // Сравнит Id.
                        var isOwner = userId.Equals(franchise.UserId);
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
        /// <returns>Данные франшизы.</returns>
        private InitGarantDataOutput GetDataFranchise1IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId)
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
                OtherId = userId
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
        /// <returns>Данные франшизы.</returns>
        private InitGarantDataOutput GetDataFranchise1IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId)
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
                OtherId = userId
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
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise2IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account)
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
                IterationList = iterationList
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
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise2IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account)
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
                IterationList = iterationList
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
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise3IterationNotOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account)
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
                IsOwner = false
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
        /// <returns>Данные франшизы.</returns>
        private async Task<InitGarantDataOutput> GetDataFranchise3IterationOwner(long franchiseId, string totalInvest, string title, string url, string firstName, string lastName, string otherFirstName, string otherLastName, string orderType, string userId, string investInclude, List<ConvertInvestIncludePriceOutput> iterationList, bool isChat, string account)
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
                IsOwner = true
            };

            if (isChat && !string.IsNullOrEmpty(userId))
            {
                // Получит данные для чата.
                var messages = await _chatService.GetDialogAsync(null, account, userId, orderType);

                result.ChatData = messages;
            }

            return result;
        }
    }
}
