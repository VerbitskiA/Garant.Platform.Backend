using System;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;

namespace Garant.Platform.Commerce.Abstraction.Tinkoff
{
    /// <summary>
    /// Абстракция сервиса платежной системы Тинькофф.
    /// </summary>
    public interface ITinkoffService
    {
        /// <summary>
        /// Метод холдирует платеж на определенный срок, пока не получит подтверждения оплаты.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="endDate">Дата действия холдирования.</param>
        /// <param name="description">Объект с описанием.</param>
        /// <param name="redirectUrl">Ссылка редиректа после успешного холдирования.</param>
        /// <returns>Данные холдирования платежа.</returns>
        Task<HoldPaymentOutput> HoldPaymentAsync(long orderId, double amount, DateTime endDate, Description description, string redirectUrl);
    }
}
