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
        /// <returns>Данные сделки.</returns>
        Task<DealOutput> CreateDealAsync(long itemDealId, string userId);

        /// <summary>
        /// Метод проверит существование открытой сделки с таким предметом сделки.
        /// </summary>
        /// <param name="itemDealId">Id предмета сделки.</param>
        /// <param name="userId">Id пользователя, который создал сделку.</param>
        /// <returns>Статус проверки.</returns>
        Task<bool> CheckDealByItemDealIdAsync(long itemDealId, string userId);
    }
}
