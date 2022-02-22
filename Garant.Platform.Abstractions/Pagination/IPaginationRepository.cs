using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Franchise.Output;

namespace Garant.Platform.Abstractions.Pagination
{
    /// <summary>
    /// Абстракция репозитория пагинации.
    /// </summary>
    public interface IPaginationRepository
    {
        /// <summary>
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<List<BusinessOutput>> GetBusinessListAsync();

        /// <summary>
        /// Метод получит список бизнеса с флагом гаранта.
        /// </summary>
        /// <returns>Список бизнеса с флагом гаранта.</returns>
        Task<List<BusinessOutput>> GetBusinessesListIsGarantAsync();

        /// <summary>
        /// Метод получит список франшиз с флагом гаранта.
        /// </summary>
        /// <returns>Спсиок франшиз с флагом гаранта.</returns>
        Task<List<FranchiseOutput>> GetFranchisesListIsGarantAsync();
    }
}
