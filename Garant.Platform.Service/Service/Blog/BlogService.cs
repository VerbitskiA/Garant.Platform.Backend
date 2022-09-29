using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Models.Blog.Input;
using Garant.Platform.Models.Blog.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Garant.Platform.Services.Service.Blog
{
    /// <summary>
    /// Сервис блогов и новостей.
    /// </summary>
    public sealed class BlogService : IBlogService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IBlogRepository _blogRepository;
        private readonly IFtpService _ftpService;

        public BlogService(IBlogRepository blogRepository, IFtpService ftpService)
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
            _blogRepository = blogRepository;
            _ftpService = ftpService;
        }

        /// <summary>
        /// /// Метод получит список блогов, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список блогов.</returns>
        public async Task<IEnumerable<BlogOutput>> GetBlogsListMainPageAsync()
        {
            try
            {
                //TODO: сделать через репозиторий
                var result = await (from b in _postgreDbContext.Blogs
                                    where b.IsPaid.Equals(true)
                                    select new BlogOutput
                                    {
                                        BlogId = b.BlogId,
                                        Title = b.Title,
                                        Url = b.Url,
                                        IsPaid = b.IsPaid,
                                        Position = b.Position,
                                        DateCreated = b.DateCreated,
                                        ThemeCategoryCode = b.ThemeCategoryCode
                                    })
                    .Take(3)
                    .ToListAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список новостей, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список новостей.</returns>
        public async Task<IEnumerable<NewsOutput>> GetTopNewsMainPageAsync()
        {
            try
            {
                //TODO: сделать через репозиторий
                var result = await (from n in _postgreDbContext.News
                                    where n.IsPaid.Equals(true)
                                    select new NewsOutput
                                    {
                                        NewsId = n.NewsId,
                                        Text = n.Text,
                                        DateCreated = n.DateCreated,
                                        IsPaid = n.IsPaid,
                                        Title = n.Title,
                                        Type = n.Type,
                                        Url = n.Url,
                                        Position = n.Position
                                    })
                    .ToListAsync();

                // Вычислит поля даты и времени.
                var i = 0;
                var nowDay = DateTime.Now.Day;

                foreach (var item in result)
                {
                    // Если день совпадает с сегодня, то проставит флаг и надпись.
                    if (item.DateCreated.Day == nowDay)
                    {
                        item.Date = "сегодня";
                    }

                    else
                    {
                        // 17 июля
                        item.Date = string.Format("{0:m}", item.DateCreated);
                    }

                    // Вычислит часы и минуты.
                    item.Time = string.Format("{0:t}", item.DateCreated);

                    i++;
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список тем блогов.
        /// </summary>
        /// <returns>Список тем блогов.</returns>
        public async Task<IEnumerable<BlogThemesOutput>> GetBlogThemesAsync()
        {
            try
            {
                var result = await _blogRepository.GetBlogThemesAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        public async Task<IEnumerable<BlogOutput>> GetBlogsListAsync()
        {
            try
            {
                var result = await _blogRepository.GetBlogsListAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст новый блог.
        /// </summary>
        /// <param name="blogData">Входные данные блога.</param>
        /// <param name="images">Файлы изображений.</param>
        /// <returns>Созданный блог.</returns>
        public async Task<BlogOutput> CreateBlogAsync(string blogData, IFormCollection images)
        {
            try
            {
                BlogOutput result = null;

                if (images.Files.Any())
                {
                    var blogInput = JsonConvert.DeserializeObject<CreateBlogInput>(blogData);

                    if (blogInput != null)
                    {
                        // создаст блог в БД
                        result = await _blogRepository.CreateBlogAsync(blogInput.Title,
                            "../../../assets/images/" + images.Files[0].FileName, blogInput.ThemeCategoryCode);
                    }
                }

                if (result != null)
                {
                    // Загрузит изображение на сервер.
                    await _ftpService.UploadFilesFtpAsync(images.Files);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод обновит существующий блог.
        /// </summary>
        /// <param name="blogData">Входные данные блога.</param>
        /// <param name="images">Файлы изображений.</param>
        /// <returns>Обновлённый блог.</returns>
        public async Task<BlogOutput> UpdateBlogAsync(string blogData, IFormCollection images)
        {
            try
            {
                BlogOutput result = null;

                if (images.Files.Any())
                {
                    var blogInput = JsonConvert.DeserializeObject<UpdateBlogInput>(blogData);

                    if (blogInput != null)
                    {
                        // обновит блог в БД
                        result = await _blogRepository.UpdateBlogAsync(blogInput.BlogId, blogInput.Title,
                            "../../../assets/images/" + images.Files[0].FileName, blogInput.IsPaid, blogInput.Position,
                            blogInput.ThemeCategoryCode);
                    }
                }

                if (result != null)
                {
                    // Загрузит изображение на сервер.
                    await _ftpService.UploadFilesFtpAsync(images.Files);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст новость.
        /// </summary>
        /// <param name="newsData">Данные новости.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные новости.</returns>
        public async Task<NewsOutput> CreateNewsAsync(string newsData, IFormCollection images)
        {
            try
            {
                NewsOutput result = null;

                if (images.Files.Any())
                {
                    var newsInput = JsonConvert.DeserializeObject<CreateNewsInput>(newsData);

                    if (newsInput != null)
                    {
                        // создаст новость
                        result = await _blogRepository.CreateNewsAsync(newsInput.Title, newsInput.Text,
                            "../../../assets/images/" + images.Files[0].FileName, newsInput.Type);
                    }
                }

                if (result != null)
                {
                    // Загрузит изображение на сервер.
                    await _ftpService.UploadFilesFtpAsync(images.Files);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод обновит новость.
        /// </summary>
        /// <param name="newsData">Данные новости.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные новости.</returns>
        public async Task<NewsOutput> UpdateNewsAsync(string newsData, IFormCollection images)
        {
            try
            {
                NewsOutput result = null;

                if (images.Files.Any())
                {
                    var newsInput = JsonConvert.DeserializeObject<UpdateNewsInput>(newsData);

                    if (newsInput != null)
                    {
                        // обновит новость в БД
                        result = await _blogRepository.UpdateNewsAsync(newsInput.NewsId, newsInput.Title, newsInput.Text,
                            "../../../assets/images/" + images.Files[0].FileName, newsInput.Type);
                    }
                }

                if (result != null)
                {
                    // Загрузит изображение на сервер.
                    await _ftpService.UploadFilesFtpAsync(images.Files);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст статью.
        /// </summary>
        /// <param name="articleData">Данные статьи.</param>
        /// <param name="images">Данные статьи.</param>
        /// <returns>Данные статьи.</returns>
        public async Task<ArticleOutput> CreateArticleAsync(string articleData, IFormCollection images)
        {
            try
            {
                ArticleOutput result = null;

                if (images.Files.Any())
                {
                    var articleInput = JsonConvert.DeserializeObject<CreateArticleInput>(articleData);

                    if (articleInput != null)
                    {
                        // Создаст статью.
                        var previewFileName = images.Files.FirstOrDefault(f => f.Name.Equals("previewFile"))?.FileName;
                        var articleFileName = images.Files.FirstOrDefault(f => f.Name.Equals("articleFile"))?.FileName;

                        result = await _blogRepository.CreateArticleAsync(articleInput.BlogId, previewFileName,
                            articleFileName, articleInput.Title, articleInput.Description, articleInput.Text,
                            Guid.NewGuid().ToString(), articleInput.SignatureText);
                    }
                }

                if (result != null)
                {
                    // Загрузит изображение на сервер.
                    await _ftpService.UploadFilesFtpAsync(images.Files);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод обновит статью.
        /// </summary>
        /// <param name="articleData">Данные статьи.</param>
        /// <param name="images">Изображения статьи.</param>
        /// <returns>Данные статьи.</returns>
        public async Task<ArticleOutput> UpdateArticleAsync(string articleData, IFormCollection images)
        {
            try
            {
                ArticleOutput result = null;

                if (images.Files.Any())
                {
                    var articleInput = JsonConvert.DeserializeObject<UpdateArticleInput>(articleData);


                    if (articleInput != null)
                    {
                        string[] urls = new string[images.Files.Count];

                        for (int i = 0; i < images.Files.Count; i++)
                        {
                            urls[i] = images.Files[i].FileName;
                        }

                        // обновит статью в БД
                        result = await _blogRepository.UpdateArticleAsync(articleInput.ArticleId, articleInput.BlogId,
                            urls, articleInput.Title, articleInput.Position, articleInput.Description,
                            articleInput.Text, articleInput.ArticleCode);
                    }
                }

                if (result != null)
                {
                    // Загрузит изображение на сервер.
                    await _ftpService.UploadFilesFtpAsync(images.Files);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод вернёт список новостей упорядоченный по дате создания.
        /// </summary>
        /// <returns>Список новостей упорядоченный по дате создания. </returns>
        public async Task<IEnumerable<NewsOutput>> GetNewsListAsync()
        {
            try
            {
                var result = await _blogRepository.GetNewsListAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод вернёт список статей, относящихся к блогу, упорядоченный по дате создания.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns>Список статей упорядоченный по дате создания.</returns>
        public async Task<IEnumerable<ArticleOutput>> GetArticlesFromBlogAsync(long blogId)
        {
            try
            {
                var result = await _blogRepository.GetArticlesFromBlogAsync(blogId);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список тем для статей блогов.
        /// </summary>
        /// <returns>Список тем.</returns>
        public async Task<IEnumerable<ArticleThemeOutput>> GetArticleThemesAsync()
        {
            try
            {
                var result = await _blogRepository.GetArticleThemesAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит блог по его Id.
        /// </summary>
        /// <param name="blogId">Id блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> GetBlogAsync(long blogId)
        {
            try
            {
                if (blogId <= 0)
                {
                    throw new EmptyBlogIdException();
                }

                var blog = await _blogRepository.GetBlogByIdAsync(blogId);
                var mapper = AutoFac.Resolve<IMapper>();
                var result = mapper.Map<BlogOutput>(blog);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит статью блога.
        /// </summary>
        /// <param name="articleId">Id статьи.</param>
        /// <returns>Данные статьи.</returns>
        public async Task<ArticleOutput> GetBlogArticleAsync(long articleId)
        {
            try
            {
                if (articleId <= 0)
                {
                    throw new NotFoundArticleByIdException(articleId);
                }

                var article = await _blogRepository.GetBlogArticleByUdAsync(articleId);

                var cast = AutoFac.Resolve<IMapper>();
                var result = cast.Map<ArticleOutput>(article);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит новость по ее Id.
        /// </summary>
        /// <param name="newsId">Id новости.</param>
        /// <returns>Данные новости.</returns>
        public async Task<NewsOutput> GetNewAsync(long newsId)
        {
            try
            {
                if (newsId <= 0)
                {
                    throw new EmptyNewsIdException();
                }

                var getNew = await _blogRepository.GetNewByIdAsync(newsId);
                var cast = AutoFac.Resolve<IMapper>();
                var result = cast.Map<NewsOutput>(getNew);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод удалит новость.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns></returns>
        public async Task DeleteNewAsync(long newsId)
        {
            try
            {
                await _blogRepository.DeleteNewAsync(newsId);                
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод удалит статью.
        /// </summary>
        /// <param name="articleId">Идентификатор статьи.</param>
        /// <returns></returns>
        public async Task DeleteArticleAsync(long articleId)
        {
            try
            {
                await _blogRepository.DeleteArticleAsync(articleId);               
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод удалит блог со статьями.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns></returns>
        public async Task DeleteBlogAsync(long blogId)
        {
            try
            {
                await _blogRepository.DeleteBlogAsync(blogId);                
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод увеличит счётчик просмотров новости один раз в сутки на пользователя.
        /// </summary>
        /// <param name="account">Данные об аккаунте пользователя.</param>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns></returns>
        public async Task<bool> IncrementViewsNewOnceADayAsync(string account, long newsId)
        {
            try
            {
                var res = await _blogRepository.IncrementViewsNewOnceADayAsync(account, newsId); 

                return res;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод увеличит счётчик просмотров блога один раз в сутки на пользователя.
        /// </summary>
        /// <param name="account">Данные об аккаунте пользователя.</param>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns></returns>
        public async Task<bool> IncrementViewsBlogOnceADayAsync(string account, long blogId)
        {
            try
            {
                var res = await _blogRepository.IncrementViewsBlogOnceADayAsync(account, blogId);

                return res;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод увеличит счётчик просмотров статьи один раз в сутки на пользователя.
        /// </summary>
        /// <param name="account">Данные об аккаунте пользователя.</param>
        /// <param name="articleId">Идентификатор статьи.</param>
        /// <returns></returns>
        public async Task<bool> IncrementViewsArticleOnceADayAsync(string account, long articleId)
        {
            try
            {
                var res = await _blogRepository.IncrementViewsArticleOnceADayAsync(account, articleId);

                return res;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}