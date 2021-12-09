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
    }
}
