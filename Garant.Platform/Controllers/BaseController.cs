using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    [ApiController]
    [Route("test")]
    public class BaseController : Controller
    {
        [HttpGet, Route("gettest")]
        public IActionResult GetTest()
        {
            return Ok();
        }
    }
}
