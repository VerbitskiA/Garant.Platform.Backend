using Garant.Platform.Base;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Test
{
    [ApiController]
    [Route("search")]
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