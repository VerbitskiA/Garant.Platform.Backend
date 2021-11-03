using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Entities.Franchise;
using Garant.Platform.Models.Franchise.Input;
using Garant.Platform.Models.Franchise.Output;
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
        /// <returns>Список франшиз после фильтрации.</returns>
        Task<List<FranchiseOutput>> FilterFranchisesAsync(string typeSort, string minPrice, string maxPrice,
            bool isGarant = false);

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
            CreateUpdateFranchiseInput franchiseInput, long lastFranchiseId, List<string> urlsDetails,
            IFormFileCollection franchiseFilesInput, string account);

        /// <summary>
        /// Метод найдет франшизу по названию.
        /// </summary>
        /// <param name="title">Название франшизы.</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseEntity> FindFranchiseByTitleAsync(string title);

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные франшизы.</returns>
        Task<FranchiseOutput> GetFranchiseAsync(long franchiseId, string mode);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateFranchiseAsync(IFormFileCollection files, string account);
    }
}
