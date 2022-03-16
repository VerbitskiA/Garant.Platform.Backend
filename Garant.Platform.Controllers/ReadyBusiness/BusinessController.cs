using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Base;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Franchise.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.ReadyBusiness
{
    /// <summary>
    /// Контроллер готового бизнеса.
    /// </summary>
    [ApiController]
    [Route("business")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        [HttpPost]
        [Route("create-update-business")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateBusinessOutput))]
        public async Task<IActionResult> CreateUpdateBusinessAsync([FromForm] IFormCollection businessFilesInput,
            [FromForm] string businessDataInput)
        {
            var result =
                await _businessService.CreateUpdateBusinessAsync(businessFilesInput, businessDataInput, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод отправит файл в папку и запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        [HttpPost]
        [Route("temp-file")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> AddTempFilesBeforeCreateBusinessAsync([FromForm] IFormCollection files)
        {
            var result = await _businessService.AddTempFilesBeforeCreateBusinessAsync(files, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessInput">Входная модель.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("get-business")]
        [ProducesResponseType(200, Type = typeof(BusinessOutput))]
        public async Task<IActionResult> GetFranchiseAsync([FromBody] BusinessInput businessInput)
        {
            var result = await _businessService.GetBusinessAsync(businessInput.BusinessId, businessInput.Mode);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("category-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetBusinessCategoryOutput>))]
        public async Task<IActionResult> GetBusinessCategoriesAsync()
        {
            var result = await _businessService.GetBusinessCategoriesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("subcategory-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessSubCategoryOutput>))]
        public async Task<IActionResult> GetBusinessSubCategoryListAsync()
        {
            var result = await _businessService.GetSubBusinessCategoryListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("cities-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessCitiesOutput>))]
        public async Task<IActionResult> GetCitiesListAsync()
        {
            var result = await _businessService.GetCitiesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список популярного бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("popular-business")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PopularBusinessOutput>))]
        public async Task<IActionResult> GetPopularBusinessAsync()
        {
            var result = await _businessService.GetPopularBusinessAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("catalog-business")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PopularBusinessOutput>))]
        public async Task<IActionResult> GetBusinessListAsync()
        {
            var result = await _businessService.GetBusinessListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список бизнеса на основе фильтров.
        /// </summary>
        /// <param name="filterInput">Входная модель.</param>
        /// <returns>Список бизнеса.</returns>
        [AllowAnonymous]
        [HttpPost, Route("filter-businesses")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessOutput>))]
        public async Task<IActionResult> FilterBusinessesAsync([FromBody] FilterInput filterInput)
        {
            var result = await _businessService.FilterBusinessesAsync(filterInput.TypeSortPrice,
                filterInput.ProfitMinPrice, filterInput.ProfitMaxPrice, filterInput.ViewCode, filterInput.CategoryCode,
                filterInput.MinPriceInvest, filterInput.MaxPriceInvest, filterInput.IsGarant);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список бизнеса на основе фильтров и данных пагинации.
        /// </summary>
        /// <param name="filtersWithPaginationInput">Входная модель фильтров с пагинацией.</param>
        /// <returns>Список бизнеса и данные пагинации.</returns>
        [AllowAnonymous]
        [HttpPost, Route("filter-pagination")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessOutput>))]
        public async Task<IActionResult> FilterBusinessesWithPaginationAsync(
            [FromBody] FilterBusinessesWithPaginationInput filtersWithPaginationInput)
        {
            var result = await _businessService.FilterBusinessesWithPaginationAsync(
                filtersWithPaginationInput.TypeSortPrice,
                filtersWithPaginationInput.MinPrice,
                filtersWithPaginationInput.MaxPrice,
                filtersWithPaginationInput.City,
                filtersWithPaginationInput.CategoryCode,
                filtersWithPaginationInput.MinProfit,
                filtersWithPaginationInput.MaxProfit,
                filtersWithPaginationInput.PageNumber,
                filtersWithPaginationInput.CountRows,
                filtersWithPaginationInput.IsGarant);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит новый бизнес, который был создан в текущем месяце.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        [AllowAnonymous]
        [HttpPost, Route("new-business")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessOutput>))]
        public async Task<IActionResult> GetNewFranchisesAsync()
        {
            var result = await _businessService.GetNewBusinesseListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод поместит бизнес в архив.
        /// </summary>
        /// <param name="businessId">Идентификатор бизнеса.</param>
        /// <returns>Статус архивации.</returns>        
        [HttpPost]
        [Route("to-archive")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> ArchiveBusinessAsync([FromBody] long businessId)
        {
            var result = await _businessService.ArchiveBusinessAsync(businessId);

            return result;
        }

        /// <summary>
        /// Метод вернёт список бизнесов из архива.
        /// </summary>
        /// <returns>Список архивированных бизнесов.</returns>        
        [HttpGet]
        [Route("archive-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessOutput>))]
        public async Task<IEnumerable<BusinessOutput>> GetArchiveFranchiseListAsync()
        {
            var result = await _businessService.GetArchiveBusinessListAsync();

            return result;
        }

        /// <summary>
        /// Метод восстановит бизнес из архива.
        /// </summary>
        /// <param name="businessId">Идентификатор бизнеса.</param>
        /// <returns>Статус восстановления бизнеса.</returns>       
        [HttpPost]
        [Route("restore-from-archive")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> RestoreFranchiseFromArchive([FromBody] long businessId)
        {
            var result = await _businessService.RestoreBusinessFromArchive(businessId);

            return result;
        }

        /// <summary>
        /// Метод удалит из архива бизнесы, которые там находятся больше одного месяца (>=31 дней).
        /// </summary>
        /// <returns>Бизнесы в архиве после удаления.</returns>            
        [HttpPost]
        [Route("remove-older-month")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BusinessOutput>))]
        public async Task<IEnumerable<BusinessOutput>> RemoveFranchisesOlderMonthFromArchiveAsync()
        {
            var result = await _businessService.RemoveBusinessesOlderMonthFromArchiveAsync();

            return result;
        }
    }
}