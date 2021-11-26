using System.Collections;
using System.Threading.Tasks;

namespace Garant.Platform.Abstractions.Search
{
    /// <summary>
    /// Абстракция поиска.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Метод найдет среди франшиз по запросу.
        /// </summary>
        /// <param name="searchType">Тип поиска.</param>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список с результатами.</returns>
        Task<IEnumerable> SearchByFranchisesAsync(string searchType, string searchText);
    }
}
