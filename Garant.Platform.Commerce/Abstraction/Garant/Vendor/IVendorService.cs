using System.Threading.Tasks;
using Garant.Platform.Models.Commerce.Output;

namespace Garant.Platform.Commerce.Abstraction.Garant.Vendor
{
    /// <summary>
    /// Абстракция сервиса со стороны продавца.
    /// </summary>
    public interface IVendorService
    {
        /// <summary>
        /// Метод подтверждает начало сделки. Продавец подтверждает продажу.
        /// </summary>
        /// <param name="itemDealId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="orderType">Тип предмета сделки (франшиза или бизнес).</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные сделки.</returns>
        Task<DealOutput> AcceptActualDealAsync(long itemDealId, string orderType, string account);
    }
}
