using System.Collections;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Search;
using Garant.Platform.Base;
using Garant.Platform.Models.Search.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Search
{
    /// <summary>
    /// Контроллер поиска.
    /// </summary>
    [ApiController]
    [Route("search")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Метод найдет среди франшиз по запросу.
        /// </summary>
        /// <param name="searchInput">Входная модель.</param>
        /// <returns>Список с результатами.</returns>
        [HttpPost]
        [Route("search-data")]
        [ProducesResponseType(200, Type = typeof(IEnumerable))]
        public async Task<IActionResult> SearchAsync([FromBody] SearchInput searchInput)
        {
            var result = await _searchService.SearchAsync(searchInput.SearchType, searchInput.SearchText);

            return Ok(result);
        }
    }
}
