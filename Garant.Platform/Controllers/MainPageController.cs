using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Category.Output;
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
    }
}
