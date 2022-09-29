using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Pagination.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Business
{
    /// <summary>
    /// Абстракция сервиса готового бизнеса.
    /// </summary>
    public interface IBusinessService
    {
        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(IFormCollection businessFilesInput, string businessDataInput, string account);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormCollection form, string account);

        /// <summary>
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        Task<BusinessOutput> GetBusinessAsync(long businessId, string mode = null);

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<IEnumerable<GetBusinessCategoryOutput>> GetBusinessCategoriesAsync();

        /// <summary>
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        Task<IEnumerable<BusinessSubCategoryOutput>> GetSubBusinessCategoryListAsync();

        /// <summary>
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        Task<IEnumerable<BusinessCitiesOutput>> GetCitiesListAsync();

        /// <summary>
        /// Метод получит список популярного бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<PopularBusinessOutput>> GetPopularBusinessAsync();

        /// <summary>
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<BusinessOutput>> GetBusinessListAsync();

        /// <summary>
        /// Метод фильтрует список бизнесов по параметрам.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="profitMinPrice">Цена от.</param>
        /// <param name="profitMaxPrice">Цена до.</param>
        /// <param name="categoryCode">Город.</param>
        /// <param name="viewCode">Код вида бизнеса.</param>
        /// <param name="minPriceInvest">Сумма общих инвестиций от.</param>
        /// <param name="maxPriceInvest">Сумма общих инвестиций до.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации.</returns>
        Task<IEnumerable<BusinessOutput>> FilterBusinessesAsync(string typeSortPrice, double profitMinPrice, double profitMaxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = false);

        /// <summary>
        /// Метод фильтрует бизнес по параметрам с учётом пагинации.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="minPrice">Цена от.</param>
        /// <param name="maxPrice">Цена до.</param>
        /// <param name="city">Город.</param>
        /// <param name="categoryCode">Код вида бизнеса.</param>
        /// <param name="profitMinPrice">Прибыль в месяц от.</param>
        /// <param name="profitMaxPrice">прибыль в месяц до.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Количество записей.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации и данные пагинации.</returns>
        Task<IndexOutput> FilterBusinessesWithPaginationAsync(string typeSortPrice, double minPrice, double maxPrice,
                                                                           string city, string categoryCode, double profitMinPrice,
                                                                           double profitMaxPrice, int pageNumber, int countRows, bool isGarant = true);

        /// <summary>
        /// Метод получит новый бизнес, который был создан в текущем месяце.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<BusinessOutput>> GetNewBusinesseListAsync();

        /// <summary>
        /// Метод получит список бизнесов, которые ожидают согласования.
        /// </summary>
        /// <returns>Список бизнесов.</returns>
        Task<IEnumerable<BusinessOutput>> GetNotAcceptedBusinessesAsync();

        /// <summary>
        /// Метод поместит бизнес в архив.
        /// </summary>
        /// <param name="businessId">Идентификатор бизнеса.</param>
        /// <returns>Статус архивации.</returns>
        Task<bool> ArchiveBusinessAsync(long businessId);

        /// <summary>
        /// Метод вернёт список бизнесов из архива.
        /// </summary>
        /// <returns>Список архивированных бизнесов.</returns>
        Task<IEnumerable<BusinessOutput>> GetArchiveBusinessListAsync();

        /// <summary>
        /// Метод восстановит бизнес из архива.
        /// </summary>
        /// <param name="businessId">Идентификатор бизнеса.</param>
        /// <returns>Статус восстановления бизнеса.</returns>
        Task<bool> RestoreBusinessFromArchive(long businessId);

        /// <summary>
        /// Метод удалит из архива бизнесы, которые там находятся больше одного месяца (>=31 дней).
        /// </summary>
        /// <returns>Бизнесы в архиве после удаления.</returns>
        Task<IEnumerable<BusinessOutput>> RemoveBusinessesOlderMonthFromArchiveAsync();
    }
}
