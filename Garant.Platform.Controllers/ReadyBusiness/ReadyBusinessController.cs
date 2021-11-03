using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.ReadyBusiness
{
    /// <summary>
    /// Контроллер готового бизнеса.
    /// </summary>
    [ApiController]
    [Route("ready-business")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReadyBusinessController : BaseController
    {
        public ReadyBusinessController()
        {

        }

        [HttpPost]
        [Route("create-update-business")]
        public async Task<IActionResult> CreateUpdateBusinessAsync()
        {
            return Ok();
        }
    }
}
