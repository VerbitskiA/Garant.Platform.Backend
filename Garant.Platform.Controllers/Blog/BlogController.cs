using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Base;
using Garant.Platform.Models.Blog.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Blog
{
    /// <summary>
    /// Контроллер блогов и новостей.
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
        /// Метод получит список объявлений для главной страницы.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        [AllowAnonymous]
        [HttpGet, Route("main-blogs")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogOutput>))]
        public async Task<IActionResult> GetBlogsListMainPageAsync()
        {
            var result = await _blogService.GetBlogsListMainPageAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список новостей, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список новостей.</returns>
        [AllowAnonymous]
        [HttpGet, Route("main-news")]
        public async Task<IActionResult> GetTopNewsMainPageAsync()
        {
            var result = await _blogService.GetTopNewsMainPageAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список тем блогов.
        /// </summary>
        /// <returns>Список тем блогов.</returns>
        [AllowAnonymous]
        [HttpGet, Route("get-blog-themes")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogThemesOutput>))]
        public async Task<IActionResult> GetBlogThemesListAsync()
        {
            var result = await _blogService.GetBlogThemesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        [AllowAnonymous]
        [HttpGet, Route("get-blogs")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogOutput>))]
        public async Task<IActionResult> GetBlogsListAsync()
        {
            var result = await _blogService.GetBlogsListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод создаст новый блог.
        /// </summary>
        /// <param name="blogData">Входная модель блога.</param>
        /// <param name="images">Загружаемые изображения.</param>
        /// <returns>Созданный блог.</returns>
        [AllowAnonymous]
        [HttpPost, Route("create-blog")]
        [ProducesResponseType(200, Type = typeof(BlogOutput))]
        public async Task<IActionResult> CreateBlogAsync([FromForm] string blogData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.CreateBlogAsync(blogData, images);

            return Ok(result);
        }

        /// <summary>
        /// Метод обновит существующий блог.
        /// </summary>
        /// <param name="blogData">Входная модель блога.</param>
        /// <param name="images">Загружаемые изображения.</param>
        /// <returns>Обновлённый блог.</returns>
        [AllowAnonymous]
        [HttpPut, Route("update-blog")]
        [ProducesResponseType(200, Type = typeof(BlogOutput))]
        public async Task<IActionResult> UpdateBlogAsync([FromForm] string blogData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.UpdateBlogAsync(blogData, images);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost, Route("create-news")]
        [ProducesResponseType(200, Type = typeof(NewsOutput))]
        public async Task<IActionResult> CreateNewsAsync([FromForm] string newsData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.CreateNewsAsync(newsData, images);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut, Route("update-news")]
        [ProducesResponseType(200, Type = typeof(NewsOutput))]
        public async Task<IActionResult> UpdateNewsAsync([FromForm] string newsData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.UpdateNewsAsync(newsData, images);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost, Route("create-article")]
        [ProducesResponseType(200, Type = typeof(ArticleOutput))]
        public async Task<IActionResult> CreateArticleAsync([FromForm] string articleData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.CreateArticleAsync(articleData, images);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut, Route("update-article")]
        [ProducesResponseType(200, Type = typeof(ArticleOutput))]
        public async Task<IActionResult> UpdateArticleAsync([FromForm] string articleData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.UpdateArticleAsync(articleData, images);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet, Route("get-news")]
        [ProducesResponseType(200, Type = typeof(NewsOutput))]
        public async Task<IActionResult> GetNewsListAsync()
        {
            var result = await _blogService.GetNewsListAsync();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet, Route("get-blog-articles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticleOutput>))]
        public async Task<IActionResult> GetArticlesFromBlogAsync(long blogId)
        {
            var result = await _blogService.GetArticlesFromBlogAsync(blogId);

            return Ok(result);
        }
    }
}
