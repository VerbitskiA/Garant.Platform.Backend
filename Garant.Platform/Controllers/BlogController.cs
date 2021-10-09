using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Blog.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    /// <summary>
    /// Контроллер блога.
    /// </summary>
    [ApiController, Route("blog")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// Метод получит список объявлений.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        [AllowAnonymous]
        [HttpPost, Route("blogs")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogOutput>))]
        public async Task<IActionResult> GetBlogsListAsync()
        {
            var result = await _blogService.GetBlogsListAsync();

            return Ok(result);
        }
    }
}
