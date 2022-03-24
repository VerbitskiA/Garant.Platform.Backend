using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Base;
using Garant.Platform.Models.Blog.Input;
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
        [HttpPost, Route("blogs")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogOutput>))]
        public async Task<IActionResult> GetBlogsListMainPageAsync()
        {
            var result = await _blogService.GetBlogsListMainPageAsync();

            return Ok(result);
        }

        /// <summary>
        /// TODO: вернуться и переделать, когда будет разработана система лояльности.
        /// Метод получит список новостей, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список новостей.</returns>
        [AllowAnonymous]
        [HttpPost, Route("pay-news")]
        public async Task<IActionResult> GetTopNewsMainPageAsync()
        {
            // var result = await _blogService.GetTopNewsMainPageAsync();
            //
            // return Ok(result);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод получит список тем блогов.
        /// </summary>
        /// <returns>Список тем блогов.</returns>
        [AllowAnonymous]
        [HttpPost, Route("blog-themes")]
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
        [HttpPost, Route("get-blogs")]
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

        /// <summary>
        /// Метод создаст новость.
        /// </summary>
        /// <param name="newsData">Входная модель новости.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные новости.</returns>
        [AllowAnonymous]
        [HttpPost, Route("create-new")]
        [ProducesResponseType(200, Type = typeof(NewsOutput))]
        public async Task<IActionResult> CreateNewsAsync([FromForm] string newsData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.CreateNewsAsync(newsData, images);

            return Ok(result);
        }

        /// <summary>
        /// Метод обновит новость.
        /// </summary>
        /// <param name="newsData">Входная модель новости.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные новости.</returns>
        [AllowAnonymous]
        [HttpPut, Route("update-new")]
        [ProducesResponseType(200, Type = typeof(NewsOutput))]
        public async Task<IActionResult> UpdateNewsAsync([FromForm] string newsData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.UpdateNewsAsync(newsData, images);

            return Ok(result);
        }

        /// <summary>
        /// Метод создаст статью.
        /// </summary>
        /// <param name="articleData">Входная модель статьи.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные статьи.</returns>
        [AllowAnonymous]
        [HttpPost, Route("create-article")]
        [ProducesResponseType(200, Type = typeof(ArticleOutput))]
        public async Task<IActionResult> CreateArticleAsync([FromForm] string articleData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.CreateArticleAsync(articleData, images);

            return Ok(result);
        }

        /// <summary>
        /// Метод обновит статью.
        /// </summary>
        /// <param name="articleData">Входная модель статьи.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные статьи.</returns>
        [AllowAnonymous]
        [HttpPut, Route("update-article")]
        [ProducesResponseType(200, Type = typeof(ArticleOutput))]
        public async Task<IActionResult> UpdateArticleAsync([FromForm] string articleData, [FromForm] IFormCollection images)
        {
            var result = await _blogService.UpdateArticleAsync(articleData, images);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список новостей упорядоченыый по дате создания.
        /// </summary>
        /// <returns>Список новостей порядоченыый по дате создания.</returns>
        [AllowAnonymous]
        [HttpPost, Route("get-news")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsOutput>))]
        public async Task<IActionResult> GetNewsListAsync()
        {
            var result = await _blogService.GetNewsListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список статей блога упорядоченыый по дате создания.
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns>Список статей блога упорядоченыый по дате создания.</returns>
        [AllowAnonymous]
        [HttpPost, Route("get-blog-articles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticleOutput>))]
        public async Task<IActionResult> GetArticlesFromBlogAsync([FromBody] GetBlogArticlesInput getBlogArticlesInput)
        {
            var result = await _blogService.GetArticlesFromBlogAsync(getBlogArticlesInput.BlogId);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список тем для статей блогов.
        /// </summary>
        /// <returns>Список тем.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("get-article-themes")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticleThemeOutput>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IEnumerable<ArticleThemeOutput>> GetArticleThemesAsync()
        {
            var result = await _blogService.GetArticleThemesAsync();

            return result;
        }

        /// <summary>
        /// Метод получит блог по его Id.
        /// </summary>
        /// <param name="blogId">Id блога.</param>
        /// <returns>Данные блога.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("get-blog")]
        [ProducesResponseType(200, Type = typeof(BlogOutput))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<BlogOutput> GetBlogAsync([FromQuery] long blogId)
        {
            var result = await _blogService.GetBlogAsync(blogId);

            return result;
        }
        
        /// <summary>
        /// Метод получит статью блога.
        /// </summary>
        /// <param name="articleId">Id статьи.</param>
        /// <returns>Данные статьи.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("get-article")]
        [ProducesResponseType(200, Type = typeof(ArticleOutput))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<ArticleOutput> GetBlogArticleAsync([FromQuery] long articleId)
        {
            var result = await _blogService.GetBlogArticleAsync(articleId);

            return result;
        }

        /// <summary>
        /// Метод получит новость по ее Id.
        /// </summary>
        /// <param name="newsId">Id новости.</param>
        /// <returns>Данные новости.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("get-new")]
        [ProducesResponseType(200, Type = typeof(NewsOutput))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<NewsOutput> GetNewAsync([FromQuery] long newsId)
        {
            var result = await _blogService.GetNewAsync(newsId);

            return result;
        }

        /// <summary>
        /// Метод удалит новость.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete]
        [Route("delete-new")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]        
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteNewAsync([FromQuery] long newsId)
        {
            await _blogService.DeleteNewAsync(newsId);

            return Ok();
        }

        /// <summary>
        /// Метод удалит статью.
        /// </summary>
        /// <param name="articleId">Идентификатор статьи.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete]
        [Route("delete-article")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]        
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteArticleAsync([FromQuery] long articleId)
        {
            await _blogService.DeleteArticleAsync(articleId);

            return Ok();
        }

        /// <summary>
        /// Метод удалит блог со всеми его статьями.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete]
        [Route("delete-blog")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]        
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBlogAsync([FromQuery] long blogId)
        {
            await _blogService.DeleteBlogAsync(blogId);

            return Ok();
        }


        /// <summary>
        /// Метод увеличит счётчик просмотров новости на одного пользователя в сутки.
        /// </summary>
        /// <param name="newId">Идентификатор новости.</param>
        /// <returns>Статус повышения количества просмотров.</returns>        
        [HttpPost]
        [Route("increment-view-new")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]        
        [ProducesResponseType(500)]
        public async Task<bool> IncrementViewsNewOnceADayAsync([FromBody] long newId)
        {
            var res = await _blogService.IncrementViewsNewOnceADayAsync(GetUserName(), newId);

            return res;           
        }

        /// <summary>
        /// Метод увеличит счётчик просмотров блога на одного пользователя в сутки.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns>Статус повышения количества просмотров.</returns>        
        [HttpPost]
        [Route("increment-view-blog")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<bool> IncrementViewsBlogOnceADayAsync([FromBody] long blogId)
        {
            var res = await _blogService.IncrementViewsBlogOnceADayAsync(GetUserName(), blogId);

            return res;
        }


        /// <summary>
        /// Метод увеличит счётчик просмотров статьи на одного пользователя в сутки.
        /// </summary>
        /// <param name="articleId">Идентификатор статьи.</param>
        /// <returns>Статус повышения количества просмотров.</returns>        
        [HttpPost]
        [Route("increment-view-article")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<bool> IncrementViewsArticleOnceADayAsync([FromBody] long articleId)
        {
            var res = await _blogService.IncrementViewsArticleOnceADayAsync(GetUserName(), articleId);

            return res;
        }
    }
}
