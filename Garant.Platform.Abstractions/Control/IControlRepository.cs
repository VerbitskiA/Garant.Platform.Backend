using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Control.Output;

namespace Garant.Platform.Abstractions.Control
{
    /// <summary>
    /// Репозиторий контролов для работы с БД.
    /// </summary>
    public interface IControlRepository
    {
        /// <summary>
        /// Метод получит список названий банков для профиля.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Список названий банков.</returns>
        Task<IEnumerable<ControlOutput>> GetFilterBankNameValuesAsync(string userId);

        /// <summary>
        /// Метод найдет банки по их названию.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список названий банков.</returns>
        Task<IEnumerable<ControlOutput>> SearchFilterBankNameValueAsync(string searchText);
    }
}
