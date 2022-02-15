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
        /// <param name="lastFranchiseId">Id последней франшизы.</param>
        /// <param name="urlsDetails">Пути к доп.изображениям.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные франшизы.</returns>
        Task<CreateUpdateFranchiseOutput> CreateUpdateFranchiseAsync(
            CreateUpdateFranchiseInput franchiseInput, long lastFranchiseId, string[] urlsDetails,
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
        /// <returns>Список подкатегорий.</returns>
        Task<IEnumerable<SubCategoryOutput>> GetSubCategoryListAsync();

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
    }
}
