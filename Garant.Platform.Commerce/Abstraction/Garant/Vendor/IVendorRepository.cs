using System.Threading.Tasks;
using Garant.Platform.Models.Commerce.Output;

namespace Garant.Platform.Commerce.Abstraction.Garant.Vendor
{
    /// <summary>
    /// Абстракция репозитория Гаранта со стороны продавца для работы с БД.
    /// </summary>
    public interface IVendorRepository
    {
        /// <summary>
        /// Метод создаст новую сделку.
        /// </summary>
        /// <param name="itemDealId">Id предмета сделки.</param>
        /// <param name="userId">Id пользователя, который создал сделку.</param>
        /// <param name="isCompletedDeal">Флаг завершена ли сделка.</param>
        /// <returns>Данные сделки.</returns>
        Task<DealOutput> CreateDealAsync(long itemDealId, string userId, bool isCompletedDeal);
    }
}
