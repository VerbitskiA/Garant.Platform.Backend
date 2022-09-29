using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Base;
using Garant.Platform.Models.Franchise.Input;
using Garant.Platform.Models.Franchise.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Franchise
{
    /// <summary>
    /// Контроллер франшиз.
    /// </summary>
    [ApiController, Route("franchise")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FranchiseController : BaseController
    {
        private readonly IFranchiseService _franchiseService;

        public FranchiseController(IFranchiseService franchiseService)
        {
            _franchiseService = franchiseService;
        }

        /// <summary>
        /// Метод получит список франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost, Route("catalog-franchise")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> GetFranchisesListAsync()
        {
            var result = await _franchiseService.GetFranchisesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost, Route("main-popular")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PopularFranchiseOutput>))]
        public async Task<IActionResult> GetMainPopularFranchisesAsync()
        {
            var result = await _franchiseService.GetMainPopularFranchises();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost, Route("quick-franchises")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> GetFranchiseQuickSearchAsync()
        {
            var result = await _franchiseService.GetFranchiseQuickSearchAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод фильтрации франшиз по разным атрибутам.
        /// </summary>
        /// <param name="fitFilterInput">Входная модель.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        [AllowAnonymous]
        [HttpPost, Route("filter-franchises")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> FilterFranchisesAsync([FromBody] FilterInput fitFilterInput)
        {
            var result = await _franchiseService.FilterFranchisesAsync(fitFilterInput.TypeSortPrice,
                fitFilterInput.ProfitMinPrice, fitFilterInput.ProfitMaxPrice, fitFilterInput.ViewCode,
                fitFilterInput.CategoryCode, fitFilterInput.MinPriceInvest, fitFilterInput.MaxPriceInvest,
                fitFilterInput.IsGarant);

            return Ok(result);
        }

        /// <summary>
        /// Метод фильтрует франшизы и учётом пагинации.
        /// </summary>
        /// <param name="filterInput">Входная модель.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("filter-pagination")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> FilterFranchisesWithPaginationAsync(
            [FromBody] FilterFranchisesWithPaginationInput filterInput)
        {
            var result = await _franchiseService.FilterFranchisesWithPaginationAsync(filterInput.TypeSortPrice,
                filterInput.ViewBusinessesCode, filterInput.CategoryCode,
                filterInput.MinInvest, filterInput.MaxInvest, filterInput.MinProfit,
                filterInput.MaxProfit, filterInput.PageNumber, filterInput.CountRows, filterInput.IsGarant);
            
            return Ok(result);
        }


        /// <summary>
        /// Метод получит новые франшизы, которые были созданы в текущем месяце.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost, Route("new-franchise")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> GetNewFranchisesAsync()
        {
            var result = await _franchiseService.GetNewFranchisesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список отзывов о франшизах.
        /// </summary>
        /// <returns>Список отзывов.</returns>
        [AllowAnonymous]
        [HttpPost, Route("review")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> GetReviewsFranchisesAsync()
        {
            var result = await _franchiseService.GetReviewsFranchisesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод создаст новую или обновит существующую франшизу.
        /// </summary>
        /// <param name="franchiseFilesInput">Входные файлы.</param>
        /// <param name="franchiseDataInput">Данные в строке json.</param>
        /// <returns>Данные франшизы.</returns>
        [HttpPost, Route("create-update-franchise")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateFranchiseOutput))]
        public async Task<IActionResult> CreateUpdateFranchiseAsync([FromForm] IFormCollection franchiseFilesInput,
            [FromForm] string franchiseDataInput)
        {
            var result = await _franchiseService.CreateUpdateFranchiseAsync(franchiseFilesInput, franchiseDataInput, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseInput">Входная модель.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("get-franchise")]
        [ProducesResponseType(200, Type = typeof(FranchiseOutput))]
        public async Task<IActionResult> GetFranchiseAsync([FromQuery] long franchiseId)
        {
            var result = await _franchiseService.GetFranchiseAsync(franchiseId);

            return Ok(result);
        }

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        [HttpPost]
        [Route("temp-file")]
        public async Task<IActionResult> AddTempFilesBeforeCreateFranchiseAsync([FromForm] IFormCollection files)
        {
            var result = await _franchiseService.AddTempFilesBeforeCreateFranchiseAsync(files, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список категорий франшиз.
        /// </summary>
        /// <returns>Список категорий.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("category-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryOutput>))]
        public async Task<IActionResult> GetCategoryListAsync()
        {
            var result = await _franchiseService.GetCategoryListAsync();

            return Ok(result);
        }
        
        /// <summary>
        /// Метод получит список категорий франшиз только после авторизации.
        /// </summary>
        /// <returns>Список категорий.</returns>
        [HttpGet]
        [Route("category-list-auth")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryOutput>))]
        public async Task<IActionResult> GetCategoryListAuthAsync()
        {
            var result = await _franchiseService.GetCategoryListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список подкатегорий франшиз.
        /// </summary>
        /// <param name="categoryCode">Код категории, для которой нужно получить список подкатегорий.</param>
        /// <param name="categorySysName">Системное имя категории, для которой нужно получить список подкатегорий.</param>
        /// <returns>Список подкатегорий.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("subcategory-list")]
        public async Task<IActionResult> GetSubCategoryListAsync([FromQuery] [Required] string categoryCode,
            [Required] string categorySysName)
        {
            var result = await _franchiseService.GetSubCategoryListAsync(categoryCode, categorySysName);

            return Ok(result);
        }

        /// <summary>
        /// Метод найдет сферы в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>
        /// <returns>Список сфер.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("search-sphere")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryOutput>))]
        public async Task<IEnumerable<CategoryOutput>> SearchSphereAsync([FromQuery] [Required] string searchText)
        {
            var result = await _franchiseService.SearchSphereAsync(searchText);

            return result;
        }

        /// <summary>
        /// Метод найдет категории в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>
        /// <returns>Список сфер.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("search-category")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SubCategoryOutput>))]
        public async Task<IEnumerable<SubCategoryOutput>> SearchCategoryAsync([FromQuery] [Required] string searchText, string categoryCode, [Required] string categorySysName)
        {
            var result = await _franchiseService.SearchCategoryAsync(searchText, categoryCode, categorySysName);

            return result;
        }

        /// <summary>
        /// Метод поместит франшизу в архив.
        /// </summary>
        /// <param name="franchiseId">Идентификатор франшизы.</param>
        /// <returns>Статус архивации.</returns>
        [HttpPost]
        [Route("to-archive")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> ArchiveFranchiseAsync([FromBody] long franchiseId)
        {
            var result = await _franchiseService.ArchiveFranchiseAsync(franchiseId);

            return result;
        }

        /// <summary>
        /// Метод вернёт список франшиз из архива.
        /// </summary>
        /// <returns>Список архивированных франшиз.</returns>
        [HttpGet]
        [Route("archive-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IEnumerable<FranchiseOutput>> GetArchiveFranchiseListAsync()
        {
            var result = await _franchiseService.GetArchiveFranchiseListAsync();

            return result;
        }

        /// <summary>
        /// Метод восстановит франшизу из архива.
        /// </summary>
        /// <param name="franchiseId">Идентификатор франшизы.</param>
        /// <returns>Статус восстановления франшизы.</returns>
        [HttpPost]
        [Route("restore-from-archive")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> RestoreFranchiseFromArchive([FromBody] long franchiseId)
        {
            var result = await _franchiseService.RestoreFranchiseFromArchive(franchiseId);

            return result;
        }

        /// <summary>
        /// Метод удалит из архива франшизы, которые там находятся больше одного месяца (>=31 дней).
        /// </summary>
        /// <returns>Франшизы в архиве после удаления.</returns>        
        [HttpPost]
        [Route("remove-older-month")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IEnumerable<FranchiseOutput>> RemoveFranchisesOlderMonthFromArchiveAsync()
        {
            var result = await _franchiseService.RemoveFranchisesOlderMonthFromArchiveAsync();

            return result;
        }
    }
}