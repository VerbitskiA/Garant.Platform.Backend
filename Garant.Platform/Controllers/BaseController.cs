using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    [ApiController]
    [Route("test")]
    public class BaseController : Controller
    {
        [Route("gettest")]
        public IActionResult GetTest()
        {
            return Ok();
        }
    }
}
