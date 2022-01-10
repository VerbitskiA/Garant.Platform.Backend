using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Garant.Output;

namespace Garant.Platform.Commerce.Abstraction.Garant
{
    /// <summary>
    /// Абстракция сервиса Гаранта, где определяются различные действия.
    /// </summary>
    public interface IGarantActionService
    {
        /// <summary>
        /// Метод получит данные для стартовой страницы в Гаранте.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса, с которым зашли в Гарант.</param>
        /// <param name="orderType">Тип заказа франшиза или бизнес.</param>
        /// <param name="account">Аккаунт.</param>
        /// <param name="stage">Номер этапа.</param>
        /// <param name="isChat">Флаг чата.</param>
        /// <returns>Данные стартовой страницы.</returns>
        Task<InitGarantDataOutput> GetInitDataGarantAsync(long originalId, string orderType, string account, int stage, bool isChat);

        /// <summary>
        /// Метод выполнит платеж на счет продавцу за этап.
        /// </summary>
        Task PaymentVendorIterationAsync(string typeItemDeal, string payerAccountNumber, string currentUserId, long itemDealId, long orderId);
    }
}
