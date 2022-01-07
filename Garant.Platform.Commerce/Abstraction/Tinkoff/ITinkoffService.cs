using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Tinkoff.Output;

namespace Garant.Platform.Commerce.Abstraction.Tinkoff
{
    /// <summary>
    /// Абстракция сервиса платежной системы Тинькофф.
    /// </summary>
    public interface ITinkoffService
    {
        /// <summary>
        /// Метод снимет средства с карты покупателя за этап после вычитания комиссии.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="iterationName">Название итерации этапа.</param>
        /// <returns>Данные платежа с ссылкой на платежную форму.</returns>
        Task<PaymentInitOutput> PaymentInitAsync(long orderId, double amount, string iterationName, long dealItemId);

        /// <summary>
        /// Метод проверит статус платежа.
        /// </summary>
        /// <param name="paymentId">Id платежа в системе банка.</param>
        /// <param name="orderId">Id заказа в сервисе Гарант.</param>
        /// <returns>Данные платежа.</returns>
        Task<GetPaymentStatusOutput> GetStatePaymentAsync(string paymentId, long orderId);
    }
}
