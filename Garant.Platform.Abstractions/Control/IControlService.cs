using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Control.Output;

namespace Garant.Platform.Abstractions.Control
{
    /// <summary>
    /// Абстракция для работы с контролами.
    /// </summary>
    public interface IControlService
    {
        /// <summary>
        /// Метод получит список названий банков для профиля.
        /// </summary>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Список названий банков.</returns>
        Task<IEnumerable<ControlOutput>> GetFilterBankNameValuesAsync(string account);

        /// <summary>
        /// Метод найдет банки по их названию.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список названий банков.</returns>
        Task<IEnumerable<ControlOutput>> SearchFilterBankNameValueAsync(string searchText);
    }
}
