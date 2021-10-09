using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Franchise.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
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
        /// Метод получит список популярных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        //[HttpPost, Route("popular")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<PopularFranchiseOutput>))]
        //public async Task<IActionResult> GetPopularFranchisesAsync()
        //{
        //    var result = await _franchiseService.GetPopularFranchises();

        //    return Ok(result);
        //}

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
    }
}
