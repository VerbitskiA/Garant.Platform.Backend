using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Actions.Output;
using Garant.Platform.Models.Category.Output;
using Garant.Platform.Models.LastBuy.Output;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    /// <summary>
    /// Контроллер главной страницы.
    /// </summary>
    [ApiController, Route("main")]
    public class MainPageController : BaseController
    {
        private readonly IMainPageService _mainPageService;

        public MainPageController(IMainPageService mainPageService)
        {
            _mainPageService = mainPageService;
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий бизнеса. Все это дело разбито на 4 столбца.</returns>
        [HttpPost, Route("categories-list")]
        [ProducesResponseType(200, Type = typeof(GetResultBusinessCategoryOutput))]
        public async Task<IActionResult> GetCategoriesListAsync()
        {
            var result = await _mainPageService.GetCategoriesListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит последние 5 записей недавно купленных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
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
        [HttpPost, Route("actions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MainPageActionOutput>))]
        public async Task<IActionResult> GetActionsMainPageAsync()
        {
            var result = await _mainPageService.GetActionsMainPageAsync();

            return Ok(result);
        }
    }
}
