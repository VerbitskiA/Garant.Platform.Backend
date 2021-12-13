using System;
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
                        // Сравнит Id.
                        var isOwner = userId.Equals(franchise.UserId);

                        // Если нужно подтянуть данные не владельца предмета сделки.
                        if (!isOwner && stage == 1)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            result = new InitGarantDataOutput
                            {
                                ItemDealId = franchise.FranchiseId,
                                TotalAmount = franchise.TotalInvest,
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
                                ItemTitle = franchise.Title,
                                ContinueButtonText = "Перейти к согласованию этапов",
                                //ButtonActionText = $"Холдировать сумму - {franchise.TotalInvest} ₽",
                                ButtonActionText = string.Empty,
                                ImageUrl = franchise.Url.Split(",")[0],
                                Amount = Convert.ToDouble(franchise.TotalInvest),
                                OtherUserRole = "Продавец",
                                Role = "Покупатель (Вы)",
                                FullName = userName.FirstName + " " + userName.LastName,
                                OtherUserFullName = otherAccount.FirstName + " " + otherAccount.LastName,
                                ItemDealType = orderType,
                                OtherId = userId
                            };
                        }

                        else if (isOwner && stage == 1)
                        {
                            // Найдет Id пользователя, который оставил заявку.
                            var otherUserId = await _postgreDbContext.RequestsFranchises
                                .Where(f => f.FranchiseId == franchise.FranchiseId)
                                .Select(f => f.UserId)
                                .FirstOrDefaultAsync();

                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(otherUserId);

                            // Если нужно подтянуть данные владельца предмета сделки.
                            result = new InitGarantDataOutput
                            {
                                ItemDealId = franchise.FranchiseId,
                                TotalAmount = franchise.TotalInvest,
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
                                ItemTitle = franchise.Title,
                                ContinueButtonText = "Перейти к согласованию этапов",
                                ButtonActionText = $"Подтвердить продажу на {franchise.TotalInvest} ₽",
                                ImageUrl = franchise.Url.Split(",")[0],
                                Amount = Convert.ToDouble(franchise.TotalInvest),
                                Role = "Продавец (Вы)",
                                OtherUserRole = "Покупатель",
                                ButtonCancel = "Отменить",
                                BlockDocumentsTemplatesName = "Шаблоны документов",
                                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                                BlockDocumentDealName = "Документы сделки",
                                FullName = userName.FirstName + " " + userName.LastName,
                                OtherUserFullName = otherAccount.FirstName + " " + otherAccount.LastName,
                                ItemDealType = orderType,
                                OtherId = otherUserId
                            };
                        }

                        // Если этап 2 и не владелец.
                        else if (!isOwner && stage == 2)
                        {
                            var otherAccount = await _userRepository.GetUserProfileInfoByIdAsync(franchise.UserId);

                            result = new InitGarantDataOutput
                            {
                                ItemDealId = franchise.FranchiseId,
                                TotalAmount = franchise.TotalInvest,
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
                                BlockRightTitle = "Планирование сделки",
                                DocumentBlockTitle = "Документы сделки",
                                MainItemTitle = "Предмет сделки",
                                ItemTitle = franchise.Title,
                                ContinueButtonText = "Перейти к согласованию этапов",
                                //ButtonActionText = $"Холдировать сумму - {franchise.TotalInvest} ₽",
                                ButtonActionText = string.Empty,
                                ImageUrl = franchise.Url.Split(",")[0],
                                Amount = Convert.ToDouble(franchise.TotalInvest),
                                OtherUserRole = "Продавец",
                                Role = "Покупатель (Вы)",
                                FullName = userName.FirstName + " " + userName.LastName,
                                OtherUserFullName = otherAccount.FirstName + " " + otherAccount.LastName,
                                ItemDealType = orderType,
                                OtherId = userId
                            };
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

                            // Если нужно подтянуть данные владельца предмета сделки.
                            result = new InitGarantDataOutput
                            {
                                ItemDealId = franchise.FranchiseId,
                                TotalAmount = franchise.TotalInvest,
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
                                ItemTitle = franchise.Title,
                                ContinueButtonText = "Перейти к согласованию этапов",
                                ButtonActionText = $"Подтвердить продажу на {franchise.TotalInvest} ₽",
                                ImageUrl = franchise.Url.Split(",")[0],
                                Amount = Convert.ToDouble(franchise.TotalInvest),
                                Role = "Продавец (Вы)",
                                OtherUserRole = "Покупатель",
                                ButtonCancel = "Отменить",
                                BlockDocumentsTemplatesName = "Шаблоны документов",
                                BlockDocumentsTemplatesDetail = "Типовые документы составленные юристами GoBizy",
                                BlockDocumentDealName = "Документы сделки",
                                FullName = userName.FirstName + " " + userName.LastName,
                                OtherUserFullName = otherAccount.FirstName + " " + otherAccount.LastName,
                                ItemDealType = orderType,
                                OtherId = otherUserId
                            };
                        }

                        if (isChat && !string.IsNullOrEmpty(otherId))
                        {
                            // Получит данные для чата.
                            var messages = await _chatService.GetDialogAsync(null, account, franchise.UserId, orderType);

                            result.ChatData = messages;
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
    }
}
