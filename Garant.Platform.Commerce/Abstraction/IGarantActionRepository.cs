using System;
using System.Threading.Tasks;

namespace Garant.Platform.Commerce.Abstraction
{
    /// <summary>
    /// Абстракция репозитория событий Гаранта для работы с БД.
    /// </summary>
    public interface IGarantActionRepository
    {
        /// <summary>
        /// Метод получит дату создания сделки.
        /// </summary>
        /// <param name="userId">Id владельца предмета сделки.</param>
        /// <returns>Дату создания сделки.</returns>
        Task<DateTime> GetOwnerIdItemDealAsync(string userId);
    }
}
