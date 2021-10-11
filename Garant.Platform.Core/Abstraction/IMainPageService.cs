using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Actions.Output;
using Garant.Platform.Models.Category.Output;
using Garant.Platform.Models.Franchise.Output;
using Garant.Platform.Models.LastBuy.Output;

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
        /// <returns>Список категорий бизнеса. Все это дело разбито на 4 столбца.</returns>
        Task<GetResultBusinessCategoryOutput> GetCategoriesListAsync();

        /// <summary>
        /// Метод получит последние 5 записей недавно купленных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<LastBuyOutput>> GetSliderLastBuyAsync();

        /// <summary>
        /// Метод получит данные для блока событий главной страницы.
        /// </summary>
        /// <returns>Список данных.</returns>
        Task<IEnumerable<MainPageActionOutput>> GetActionsMainPageAsync();

        /// <summary>
        /// Метод получит список франшиз на основе фильтров.
        /// </summary>
        /// <param name="viewBusiness">Вид бизнеса.</param>
        /// <param name="category">Категория.</param>
        /// <param name="city">Город.</param>
        /// <param name="minPrice">Цена от.</param>
        /// <param name="maxPrice">Цена до.</param>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string viewBusinessCode, string categoryCode, string cityCode, double minPrice, double maxPrice);
    }
}
