using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Franchise.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Franchise
{
    /// <summary>
    /// Абстракция сервиса франшиз.
    /// </summary>
    public interface IFranchiseService
    {
        /// <summary>
        /// Метод получит список франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetFranchisesListAsync();

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

        /// <summary>
        /// Метод получит список городов франшиз.
        /// </summary>
        /// <returns>Список городов.</returns>
        Task<IEnumerable<FranchiseCityOutput>> GetFranchisesCitiesListAsync();

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<IEnumerable<CategoryOutput>> GetFranchisesCategoriesListAsync();

        /// <summary>
        /// Метод получит список видов бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<ViewBusinessOutput>> GetFranchisesViewBusinessListAsync();

        /// <summary>
        /// Метод фильтрации франшиз по разным атрибутам.
        /// </summary>
        /// <param name="typeSort">Тип фильтрации цены (по возрастанию или убыванию).</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <param name="minPrice">Прибыль от.</param>
        /// <param name="maxPrice">Прибыль до.</param>
        /// <param name="viewCode"> Код вида бизнеса.</param>
        /// <param name="categoryCode">Код категории.</param>
        /// <param name="minPriceInvest">Сумма инвестиций от.</param>
        /// <param name="maxPriceInvest">Сумма инвестиций до.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string typeSort, double minPrice, double maxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = false);

        /// <summary>
        /// Метод получит новые франшизы, которые были созданы в текущем месяце.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetNewFranchisesAsync();

        /// <summary>
        /// Метод получит список отзывов о франшизах.
        /// </summary>
        /// <returns>Список отзывов.</returns>
        Task<IEnumerable<FranchiseOutput>> GetReviewsFranchisesAsync();

        /// <summary>
        /// Метод создаст новую  или обновит существующую франшизу.
        /// </summary>
        /// <param name="franchiseFilesInput">Входные файлы.</param>
        /// <param name="franchiseDataInput">Данные в строке json.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные франшизы.</returns>
        Task<CreateUpdateFranchiseOutput> CreateUpdateFranchiseAsync(IFormCollection franchiseFilesInput, string franchiseDataInput, string account);

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseOutput> GetFranchiseAsync(long franchiseId, string mode = null);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateFranchiseAsync(IFormCollection form, string account);

        /// <summary>
        /// Метод получит список категорий франшиз.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<IEnumerable<CategoryOutput>> GetCategoryListAsync();

        /// <summary>
        /// Метод получит список подкатегорий франшиз.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        Task<IEnumerable<SubCategoryOutput>> GetSubCategoryListAsync();
    }
}
