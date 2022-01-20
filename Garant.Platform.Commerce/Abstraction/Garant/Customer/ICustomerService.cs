using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Tinkoff.Output;

namespace Garant.Platform.Commerce.Abstraction.Garant.Customer
{
    /// <summary>
    /// Абстракция сервиса со стороны покупателя.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Метод холдирует платеж на определенный срок, пока не получит подтверждения оплаты.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="orderType">Тип заказа.</param>
        /// <param name="iteration">Номер итерации этапа.</param>
        /// <returns>Данные платежа.</returns>
        Task<PaymentInitOutput> PaymentIterationCustomerAsync(long originalId, string account, string orderType, int iteration);
    }
}
