using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Output;

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
    }
}
