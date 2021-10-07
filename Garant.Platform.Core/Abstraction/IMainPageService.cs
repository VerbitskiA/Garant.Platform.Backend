using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Category.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция сервиса главной страницы.
    /// </summary>
    public interface IMainPageService
    {
        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий бизнеса.</returns>
        Task<IEnumerable<BusinessCategoryOutput>> GetCategoriesListAsync();
    }
}
