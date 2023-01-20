using Garant.Platform.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Test
{
    [ApiController]
    [Route("search")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : BaseController
    {
        [HttpPost]
        [Route("gettest")]
        public IActionResult GetTest()
        {
            return Ok("Return test from dev controller!");
        }
    }
}