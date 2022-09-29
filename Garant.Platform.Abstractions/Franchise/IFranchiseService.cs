using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Franchise.Output;
using Garant.Platform.Models.Pagination.Output;
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
        /// <param name="typeSort">Тип сортировки цены (по возрастанию или убыванию).</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <param name="minPrice">Прибыль от.</param>
        /// <param name="maxPrice">Прибыль до.</param>
        /// <param name="viewCode"> Код вида бизнеса.</param>
        /// <param name="categoryCode">Код категории.</param>
        /// <param name="minPriceInvest">Сумма инвестиций от.</param>
        /// <param name="maxPriceInvest">Сумма инвестиций до.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string typeSort, double minPrice, double maxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = true);

        /// <summary>
        /// Метод фильтрует франшизы с учётом пагинации.
        /// </summary>
        /// <param name="typeSort">Тип сортировки цены (по возрастанию или убыванию).</param>
        /// <param name="viewCode">Код вида бизнеса.</param>
        /// <param name="categoryCode">Код категории.</param>
        /// <param name="minInvest">Сумма инвестиций от.</param>
        /// <param name="maxInvest">Сумма инвестиций до.</param>
        /// <param name="minProfit">Прибыль от.</param>
        /// <param name="maxProfit">Прибыль до.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Количество объектов.</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <returns>Список франшиз после фильтрации и данные для пагинации..</returns>
        Task<IndexOutput> FilterFranchisesWithPaginationAsync(string typeSort, string viewCode, string categoryCode, double minInvest, double maxInvest, double minProfit, double maxProfit, int pageNumber, int countRows, bool isGarant);

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
        /// <param name="categoryCode">Код категории, для которой нужно получить список подкатегорий.</param>
        /// <param name="categorySysName">Системное имя категории, для которой нужно получить список подкатегорий.</param>
        /// <returns>Список подкатегорий.</returns>
        Task<IEnumerable<SubCategoryOutput>> GetSubCategoryListAsync(string categoryCode, string categorySysName);

        /// <summary>
        /// Метод найдет сферы в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>SearchCategoryAsync
        /// <returns>Список сфер.</returns>
        Task<IEnumerable<CategoryOutput>> SearchSphereAsync(string searchText);

        /// <summary>
        /// Метод найдет категории в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>
        /// <param name="categoryCode">Код сферы.</param>
        /// <param name="categorySysName">Системное название сферы.</param>
        /// <returns>Список категорий.</returns>
        Task<IEnumerable<SubCategoryOutput>> SearchCategoryAsync(string searchText, string categoryCode,
            string categorySysName);
        
        /// <summary>
        /// Метод получит список франшиз, которые ожидают согласования.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetNotAcceptedFranchisesAsync();

        /// <summary>
        /// Метод поместит франшизу в архив.
        /// </summary>
        /// <param name="franchiseId">Идентификатор франшизы.</param>
        /// <returns>Статус архивации.</returns>
        Task<bool> ArchiveFranchiseAsync(long franchiseId);

        /// <summary>
        /// Метод вернёт список франшиз из архива.
        /// </summary>
        /// <returns>Список архивированных франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetArchiveFranchiseListAsync();

        /// <summary>
        /// Метод восстановит франшизу из архива.
        /// </summary>
        /// <param name="franchiseId">Идентификатор франшизы.</param>
        /// <returns>Статус восстановления франшизы.</returns>
        Task<bool> RestoreFranchiseFromArchive(long franchiseId);

        /// <summary>
        /// Метод удалит из архива франшизы, которые там находятся больше одного месяца (>=31 дней).
        /// </summary>
        /// <returns>Франшизы в архиве после удаления.</returns>
        Task<IEnumerable<FranchiseOutput>> RemoveFranchisesOlderMonthFromArchiveAsync();
    }
}
