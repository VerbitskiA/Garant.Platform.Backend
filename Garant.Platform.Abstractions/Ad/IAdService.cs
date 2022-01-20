using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Ad.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция сервиса объявлений.
    /// </summary>
    public interface IAdService
    {
        /// <summary>
        /// Метод получит список новых объявлений.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        Task<IEnumerable<AdOutput>> GetNewAdsAsync();
    }
}
