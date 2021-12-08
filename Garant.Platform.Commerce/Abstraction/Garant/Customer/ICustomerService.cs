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
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="orderType">Тип заказа.</param>
        /// <returns>Данные холдирования платежа.</returns>
        Task<HoldPaymentOutput> HoldPaymentAsync(long originalId, double amount, string account, string orderType);
    }
}
