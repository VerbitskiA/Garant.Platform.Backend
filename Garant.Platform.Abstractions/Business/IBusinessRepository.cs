using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Entities.Business;
using Garant.Platform.Models.Request.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Business
{
    /// <summary>
    /// Абстракция репозитория готового бизнеса.
    /// </summary>
    public interface IBusinessRepository
    {
        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(CreateUpdateBusinessInput businessInput, string[] urlsBusiness, IFormFileCollection files, string account);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormFileCollection form, string account);

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        Task<BusinessOutput> GetBusinessAsync(long businessId, string mode);

        /// <summary>
        /// Метод получит бизнес по заголовку.
        /// </summary>
        /// <param name="title">Заголовок бизнеса.</param>
        /// <returns>Данные бизнеса.</returns>
        Task<BusinessEntity> GetBusinessAsync(string title);

        /// <summary>
        /// Метод получит бизнес по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные бизнеса.</returns>
        Task<BusinessEntity> GetBusinessByUserIdAsync(string userId);

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
        /// Метод получит заголовок бизнеса по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Заголовок бизнеса.</returns>
        Task<string> GetBusinessTitleAsync(string userId);

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
        Task<List<BusinessOutput>> FilterBusinessesAsync(string typeSortPrice, double profitMinPrice,
            double profitMaxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest,
            bool isGarant = false);

        /// <summary>
        /// Метод применяет фильтры независимо друг от друга.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="minPrice">Цена от.</param>
        /// <param name="maxPrice">Цена до.</param>
        /// <param name="city">Город.</param>
        /// <param name="categoryCode">Код категории.</param>        
        /// <param name="minProfit">Прибыль в месяц от.</param>
        /// <param name="maxProfit">Прибыль в месяц до.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации.</returns>
        Task<List<BusinessOutput>> FilterBusinessesIndependentlyAsync(string typeSortPrice, double minPrice, double maxPrice,
                                                                                   string city, string categoryCode, double minProfit,
                                                                                   double maxProfit, bool isGarant = true);

        /// <summary>
        /// Метод получит новый бизнес, который был создан в текущем месяце.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<List<BusinessOutput>> GetNewBusinesseListAsync();

        /// <summary>
        /// Метод найдет среди бизнеса по запросу.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список с результатами.</returns>
        Task<IEnumerable<BusinessOutput>> SearchByBusinessesAsync(string searchText);

        /// <summary>
        /// Метод создаст заявку бизнеса.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="businessId">Id бизнеса, по которому оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        Task<RequestBusinessOutput> CreateRequestBusinessAsync(string userName, string phone, string account, long businessId);
        
        /// <summary>
        /// Метод получит список заявок для вкладки профиля "Уведомления".
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Список заявок.</returns>
        Task<IEnumerable<RequestBusinessEntity>> GetBusinessRequestsAsync(string account);

        /// <summary>
        /// Метод обновит поле одобрения карточки бизнеса.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <returns>Статус одобрения.</returns>
        Task<bool> UpdateAcceptedBusinessAsync(long businessId);

        /// <summary>
        /// Метод обновит поле отклонения карточки бизнеса
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="comment">Комментарий отклонения.</param>
        /// <returns>Статус отклонения.</returns>
        Task<bool> UpdateRejectedBusinessAsync(long businessId, string comment);

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
