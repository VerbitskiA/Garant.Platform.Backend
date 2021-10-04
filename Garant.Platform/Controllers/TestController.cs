using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    [ApiController, Route("test")]
    public class TestController : ControllerBase
    {
        [HttpPost, Route("gettest")]
        public IActionResult GetTest()
        {
            return Ok(new[] {1,2,3});
        }
    }
}
