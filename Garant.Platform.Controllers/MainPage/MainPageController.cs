using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.MainPage;
using Garant.Platform.Models.Actions.Output;
using Garant.Platform.Models.Category.Output;
using Garant.Platform.Models.Franchise.Output;
using Garant.Platform.Models.LastBuy.Output;
using Garant.Platform.Models.Search.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.MainPage
{
    /// <summary>
    /// Контроллер главной страницы.
    /// </summary>
    [ApiController, Route("main")]
    [Authorize]
    public class MainPageController : ControllerBase
    {
        private readonly IMainPageService _mainPageService;
        private readonly IFranchiseService _franchiseService;

        public MainPageController(IMainPageService mainPageService, IFranchiseService franchiseService)
        {
            _mainPageService = mainPageService;
            _franchiseService = franchiseService;
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий бизнеса. Все это дело разбито на 4 столбца.</returns>
        [AllowAnonymous]
        [HttpPost, Route("categories-list")]
        [ProducesResponseType(200, Type = typeof(GetResultCategoryOutput))]
        public async Task<IActionResult> GetCategoriesListAsync()
        {
            var result = await _mainPageService.GetCategoriesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит последние 5 записей недавно купленных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost, Route("slider-last-buy")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LastBuyOutput>))]
        public async Task<IActionResult> GetSliderLastBuyAsync()
        {
            var result = await _mainPageService.GetSliderLastBuyAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит данные для блока событий главной страницы.
        /// </summary>
        /// <returns>Список данных.</returns>
        [AllowAnonymous]
        [HttpPost, Route("actions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MainPageActionOutput>))]
        public async Task<IActionResult> GetActionsMainPageAsync()
        {
            var result = await _mainPageService.GetActionsMainPageAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список франшиз на основе фильтров.
        /// </summary>
        /// <param name="quickSearchInput">Входная модель.</param>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost, Route("filter")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IActionResult> FilterFranchisesAsync([FromBody] QuickSearchInput quickSearchInput)
        {
            var result = await _mainPageService.FilterFranchisesAsync(quickSearchInput.ViewCode, quickSearchInput.CategoryCode, quickSearchInput.MinPrice, quickSearchInput.MaxPrice);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список городов франшиз.
        /// </summary>
        /// <returns>Список городов.</returns>
        [AllowAnonymous]
        [HttpPost, Route("cities-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseCityOutput>))]
        public async Task<IActionResult> GetFranchisesCitiesListAsync()
        {
            var result = await _franchiseService.GetFranchisesCitiesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        [AllowAnonymous]
        [HttpPost, Route("business-categories-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseCityOutput>))]
        public async Task<IActionResult> GetFranchisesCategoriesListAsync()
        {
            var result = await _franchiseService.GetFranchisesCategoriesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список видов бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        [AllowAnonymous]
        [HttpPost, Route("business-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ViewBusinessOutput>))]
        public async Task<IActionResult> GetFranchisesViewBusinessListAsync()
        {
            var result = await _franchiseService.GetFranchisesViewBusinessListAsync();

            return Ok(result);
        }
    }
}
