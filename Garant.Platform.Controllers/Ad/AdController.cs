using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Ad.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Ad
{
    /// <summary>
    /// Контроллер объявлений.
    /// </summary>
    [ApiController, Route("ad")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdController : BaseController
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        /// <summary>
        /// Метод получит список новых объявлений.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        [AllowAnonymous]
        [HttpPost, Route("new")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AdOutput>))]
        public async Task<IActionResult> GetNewAdsAsync()
        {
            var result = await _adService.GetNewAdsAsync();

            return Ok(result);
        }
    }
}
