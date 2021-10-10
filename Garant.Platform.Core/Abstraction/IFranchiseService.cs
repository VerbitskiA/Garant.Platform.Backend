using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Franchise.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция сервиса франшиз.
    /// </summary>
    public interface IFranchiseService
    {
        /// <summary>
        /// Метод получит список популярных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        //Task<IEnumerable<PopularFranchiseOutput>> GetPopularFranchises();

        /// <summary>
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchises();

        /// <summary>
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetFranchiseQuickSearchAsync();
    }
}
