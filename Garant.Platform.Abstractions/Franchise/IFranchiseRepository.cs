using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Entities.Franchise;
using Garant.Platform.Models.Franchise.Input;
using Garant.Platform.Models.Franchise.Output;
using Garant.Platform.Models.Request.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Franchise
{
    /// <summary>
    /// Абстракция репозитория франшиз для работы с БД.
    /// </summary>
    public interface IFranchiseRepository
    {
        /// <summary>
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<List<FranchiseOutput>> GetFranchisesAsync();

        /// <summary>
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchisesList();

        /// <summary>
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<List<FranchiseOutput>> GetFranchiseQuickSearchAsync();

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
        Task<List<FranchiseOutput>> FilterFranchisesAsync(string typeSort, double minPrice, double maxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = false);

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
        /// <returns>Список франшиз после фильтрации.</returns>
        Task<List<FranchiseOutput>> FilterFranchisesIndependentlyAsync(string typeSort, string viewCode, string categoryCode, double minInvest, double maxInvest, double minProfit, double maxProfit, int pageNumber, int countRows, bool isGarant = true);

        /// <summary>
        /// Метод получит новые франшизы, которые были созданы в текущем месяце.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<List<FranchiseOutput>> GetNewFranchisesAsync();

        /// <summary>
        /// Метод получит список отзывов о франшизах.
        /// </summary>
        /// <returns>Список отзывов.</returns>
        Task<List<FranchiseOutput>> GetReviewsFranchisesAsync();

        /// <summary>
        /// Метод создаст новую  или обновит существующую франшизу.
        /// </summary>
        /// <param name="files">Входные файлы.</param>
        /// <param name="franchiseInput">Входная модель.</param>
        /// <param name="urlsDetails">Пути к доп.изображениям.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные франшизы.</returns>
        Task<CreateUpdateFranchiseOutput> CreateUpdateFranchiseAsync(
            CreateUpdateFranchiseInput franchiseInput, string[] urlsDetails,
            IFormFileCollection franchiseFilesInput, string account);

        /// <summary>
        /// Метод найдет франшизу по названию.
        /// </summary>
        /// <param name="title">Название франшизы.</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseEntity> FindFranchiseByTitleAsync(string title);

        /// <summary>
        /// Метод найдет франшизу по Id.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseEntity> FindFranchiseByIdAsync(long franchiseId);

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseOutput> GetFranchiseAsync(long franchiseId, string mode);

        /// <summary>
        /// Метод найдет франшизу по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseEntity> FindFranchiseByUserIdAsync(string userId);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateFranchiseAsync(IFormFileCollection files, string account);

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
        /// Метод получит заголовок франшизы по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Заголовок франшизы.</returns>
        Task<string> GetFranchiseTitleAsync(string userId);

        /// <summary>
        /// Метод найдет среди франшиз по запросу.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список с результатами.</returns>
        Task<IEnumerable<FranchiseOutput>> SearchByFranchisesAsync(string searchText);

        /// <summary>
        /// Метод создаст заявку франшизы.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="city">Город.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="franchiseId">Id франшизы, по которой оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        Task<RequestFranchiseOutput> CreateRequestFranchiseAsync(string userName, string phone,
            string city, string account, long franchiseId);

        /// <summary>
        /// Метод получит список заявок по франшизам для вкладки профиля "Уведомления".
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Список заявок.</returns>
        Task<IEnumerable<RequestFranchiseEntity>> GetFranchiseRequestsAsync(string account);

        /// <summary>
        /// Метод найдет сферы в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>
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
        /// Метод обновит поле одобрения карточки франшизы.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <returns>Статус одобрения.</returns>
        Task<bool> UpdateAcceptedFranchiseAsync(long franchiseId);

        /// <summary>
        /// Метод обновит поле отклонения карточки франшизы.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="comment">Комментарий отклонения.</param>
        /// <returns>Статус отклонения.</returns>
        Task<bool> UpdateRejectedFranchiseAsync(long cardId, string comment);

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
