using System.Threading.Tasks;

namespace Garant.Platform.Commerce.Abstraction.Tinkoff
{
    /// <summary>
    /// Абстракция репозитория платежной системы Тинькофф.
    /// </summary>
    public interface ITinkoffRepository
    {
        /// <summary>
        /// Метод получит ссылку для оплаты.
        /// </summary>
        /// <returns>Ссылка на оплату.</returns>
        Task<string> GetReturnForPaymentUrlAsync();

        /// <summary>
        /// Метод запишет Id заказа в системе банка.
        /// </summary>
        /// <param name="orderId">Id заказа в сервисе Гарант.</param>
        /// <param name="systemOrderId">Id платежа в системе банка.</param>
        Task SetSystemOrderIdAsync(long orderId, long systemOrderId);
    }
}
