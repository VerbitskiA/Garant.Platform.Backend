using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Models.Entities.Franchise;
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
            var result = await _franchiseService.FilterFranchisesAsync(fitFilterInput.TypeSortPrice, fitFilterInput.ProfitMinPrice, fitFilterInput.ProfitMaxPrice, fitFilterInput.IsGarant);

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
        /// Метод создаст новую  или обновит существующую франшизу.
        /// </summary>
        /// <param name="franchiseFilesInput">Входные файлы.</param>
        /// <param name="franchiseDataInput">Данные в строке json.</param>
        /// <returns>Данные франшизы.</returns>
        [HttpPost, Route("create-update-franchise")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateFranchiseOutput))]
        public async Task<IActionResult> CreateUpdateFranchiseAsync([FromForm] IFormCollection franchiseFilesInput, [FromForm] string franchiseDataInput)
        {
            var result = await _franchiseService.CreateUpdateFranchiseAsync(franchiseFilesInput, franchiseDataInput, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseInput">Входная модель.</param>
        /// <returns></returns>
        [HttpPost, Route("get-franchise")]
        [ProducesResponseType(200, Type = typeof(FranchiseOutput))]
        public async Task<IActionResult> GetFranchiseAsync([FromBody] FranchiseInput franchiseInput)
        {
            var result = await _franchiseService.GetFranchiseAsync(franchiseInput.FranchiseId, franchiseInput.Mode);

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
    }
}
