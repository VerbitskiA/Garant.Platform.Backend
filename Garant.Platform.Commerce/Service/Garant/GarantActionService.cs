using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;

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

        public GarantActionService(PostgreDbContext postgreDbContext, IUserRepository userRepository, IFranchiseService franchiseService)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
            _franchiseService = franchiseService;
        }

        /// <summary>
        /// Метод получит данные для стартовой страницы в Гаранте.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса, с которым зашли в Гарант.</param>
        /// <param name="orderType">Тип заказа франшиза или бизнес.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные стартовой страницы.</returns>
        public async Task<InitGarantDataOutput> GetInitDataGarantAsync(long originalId, string orderType, string account)
        {
            try
            {
                // Найдет Id владельца предмета обсуждения (т.е франшизы или бизнеса).
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                var result = new InitGarantDataOutput();

                // Если заявка франшизы, то сравнит Id с владельцем франшизы.
                if (orderType.Equals("Franchise"))
                {
                    var franchise = await _franchiseService.GetFranchiseAsync(originalId);

                    if (franchise != null)
                    {
                        // Сравнит Id.
                        var isOwner = userId.Equals(franchise.UserId);

                        // Если зашел покупатель (не владелец) франшизы, то подтянет данные для покупателя.
                        if (!isOwner)
                        {
                            result = new InitGarantDataOutput
                            {
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
                                ButtonActionText = $"Холдировать сумму - {franchise.TotalInvest} ₽",
                                ImageUrl = franchise.Url.Split(",")[0],
                                Amount = Convert.ToDouble(franchise.TotalInvest)
                            };
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
