using System.Collections.Generic;
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
        /// Метод снимет средства с карты покупателя за этап после вычитания комиссии.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="iterationName">Название итерации этапа.</param>
        /// <returns>Данные платежа с ссылкой на платежную форму.</returns>
        Task<PaymentInitOutput> PaymentInitAsync(long orderId, double amount, string iterationName, long dealItemId);

        /// <summary>
        /// Метод получит статус платежей итерации этапа в система банка.
        /// </summary>
        /// <param name="statuses">Массив с данными для проверки статусов платежей в системе банка.</param>
        /// <returns></returns>
        Task GetStateAllPaymentAsync(IReadOnlyList<StatePaymentInput> statuses);

        Task GetStatePaymentAsync(string paymentId, long orderId);
    }
}
