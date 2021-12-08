using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    /// <summary>
    /// Класс реализует методы Гаранта со стороны покупателя.
    /// </summary>
    public sealed class CustomerService : ICustomerService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITinkoffService _tinkoffService;

        public CustomerService(PostgreDbContext postgreDbContex, IUserRepository userRepository, ICustomerRepository customerRepository, ITinkoffService tinkoffService)
        {
            _postgreDbContext = postgreDbContex;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _tinkoffService = tinkoffService;
        }

        /// <summary>
        /// Метод холдирует платеж на определенный срок, пока не получит подтверждения оплаты.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="orderType">Тип заказа.</param>
        /// <returns>Данные холдирования платежа.</returns>
        public async Task<HoldPaymentOutput> HoldPaymentAsync(long originalId, double amount, string account, string orderType)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                var result = new HoldPaymentOutput();

                // От текущей даты прибавит 14 дней как срок холдирования платежа.
                var endDate = DateTime.Now.AddDays(14);

                // Объект с описанием платежа.
                var description = new Description
                {
                    Short = "Тестовый платеж",
                    Full = "Полное описание тестового платежа"
                };

                var newOrder = await _customerRepository.CreateOrderAsync(originalId, amount, endDate, description, string.Empty, orderType, userId);

                // Если заказ создан успешно.
                if (newOrder != null)
                {
                    result = await _tinkoffService.HoldPaymentAsync(newOrder.OrderId, amount, endDate, description, string.Empty) ?? new HoldPaymentOutput();
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
