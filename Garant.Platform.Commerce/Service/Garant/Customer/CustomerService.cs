using System;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
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

        public CustomerService(PostgreDbContext postgreDbContex)
        {
            _postgreDbContext = postgreDbContex;
        }

        /// <summary>
        /// Метод холдирует платеж на определенный срок, пока не получит подтверждения оплаты.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="endDate">Дата действия холдирования.</param>
        /// <param name="description">Объект с описанием.</param>
        /// <param name="redirectUrl">Ссылка редиректа после успешного холдирования.</param>
        /// <returns>Данные холдирования платежа.</returns>
        public async Task<PaymentActionOutput> HoldPaymentAsync(long orderId, double amount, DateTime endDate, Description description, string redirectUrl)
        {
            try
            {
                return new PaymentActionOutput();

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
