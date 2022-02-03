using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Blog.Output;
using Garant.Platform.Models.Entities.Blog;
using Garant.Platform.Models.Entities.News;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Exceptions;

namespace Garant.Platform.Services.Service.Blog
{
    /// <summary>
    /// Репозиторий блогов и новостей.
    /// </summary>
    public class BlogRepository : IBlogRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public BlogRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод создаст новую статью в блоге.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <param name="previewUrl">Изображение превью.</param>
        /// <param name="articleUrl">Изображение статьи.</param>
        /// <param name="title">Название статьи.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="description">Описание статьи.</param>
        /// <param name="text">Полный текст статьи.</param>
        /// <param name="articleCode">Код статьи.</param>
        /// <param name="signatureText">Подпись.</param>
        /// <returns>Данные статьи.</returns>
        public async Task<ArticleOutput> CreateArticleAsync(long blogId, string previewUrl, string articleUrl,
            string title, string description, string text, string themeArticleCode, string signatureText)
        {
            try
            {
                var article = new ArticleEntity
                {
                    BlogId = blogId,
                    PreviewUrl = "../../../assets/images/" + previewUrl,
                    ArticleUrl = "../../../assets/images/" + articleUrl,
                    Title = title,
                    Description = description,
                    Text = text,
                    DateCreated = DateTime.Now,
                    ThemeCode = themeArticleCode,
                    SignatureText = signatureText,
                    ArticleCode = themeArticleCode
                };

                // Найдет последнюю позицию статей.
                var lastPosition = await _postgreDbContext.Articles
                    .OrderByDescending(o => o.BlogId)
                    .Select(b => b.Position)
                    .FirstOrDefaultAsync();

                article.Position = ++lastPosition;

                await _postgreDbContext.Articles.AddAsync(article);
                await _postgreDbContext.SaveChangesAsync();


                var jsonString = JsonConvert.SerializeObject(article);
                var result = JsonConvert.DeserializeObject<ArticleOutput>(jsonString);

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
        /// Создаст новый блог.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <param name="url">Путь к файлу.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="blogThemeCode">Код темы блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> CreateBlogAsync(string title, string url, string blogThemeCode)
        {
            try
            {
                var blog = new BlogEntity
                {
                    Title = title,
                    Url = url,
                    ThemeCategoryCode = blogThemeCode,
                    DateCreated = DateTime.Now
                };

                // Найдет последнюю позицию блогов.
                var lastPosition = await _postgreDbContext.Blogs
                    .OrderByDescending(o => o.BlogId)
                    .Select(b => b.Position)
                    .FirstOrDefaultAsync();

                blog.Position = ++lastPosition;

                await _postgreDbContext.Blogs.AddAsync(blog);
                await _postgreDbContext.SaveChangesAsync();

                var jsonString = JsonConvert.SerializeObject(blog);
                var result = JsonConvert.DeserializeObject<BlogOutput>(jsonString);

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
        /// <param name="name">Название новости.</param>
        /// <param name="text">Текст новости.</param>
        /// <param name="url">Путь к изображению.</param>
        /// <param name="isToday">Создана ли сегодня.</param>
        /// <param name="type">Тип статьи.</param>
        /// <param name="isMarginTop">Нужен ли отступ сверху.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <returns>Данные новости.</returns>
        public async Task<NewsOutput> CreateNewsAsync(string name, string text, string url, bool isToday, string type,
            bool isMarginTop, bool isPaid)
        {
            try
            {
                var news = new NewsEntity
                {
                    Name = name,
                    Text = text,
                    Url = url,
                    IsToday = isToday,
                    Type = type,
                    IsMarginTop = isMarginTop,
                    IsPaid = isPaid,
                    DateCreated = DateTime.Now
                };
                await _postgreDbContext.News.AddAsync(news);
                await _postgreDbContext.SaveChangesAsync();


                var jsonString = JsonConvert.SerializeObject(news);
                var result = JsonConvert.DeserializeObject<NewsOutput>(jsonString);

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
        /// Метод вернёт список статей, относящихся к блогу, упорядоченный по дате создания.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns>Список статей упорядоченный по дате создания.</returns>
        public async Task<IEnumerable<ArticleOutput>> GetArticlesFromBlogAsync(long blogId)
        {
            try
            {
                var result = await _postgreDbContext.Articles
                    .Where(b => b.BlogId == blogId)
                    .OrderByDescending(b =>b.DateCreated)
                    .Select(b => new ArticleOutput
                    {
                        ArticleId = b.ArticleId,
                        ArticleCode = b.ArticleCode,
                        ArticleUrl = b.ArticleUrl,
                        BlogId = b.BlogId,
                        DateCreated = b.DateCreated,
                        Description = b.Description,
                        Position = b.Position,
                        PreviewUrl = b.PreviewUrl,
                        SignatureText = b.SignatureText,
                        Text = b.Text,
                        Title = b.Title
                    })
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
        /// Метод получит блог по названию.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> GetBlogAsync(string title)
        {
            try
            {
                var result = await _postgreDbContext.Blogs
                    .Where(b => b.Title.Equals(title))
                    .Select(b => new BlogOutput
                    {
                        BlogId = b.BlogId,
                        Title = b.Title,
                        Url = b.Url,
                        IsPaid = b.IsPaid,
                        Position = b.Position,
                        DateCreated = b.DateCreated,
                        ThemeCategoryCode = b.ThemeCategoryCode
                    })
                    .FirstOrDefaultAsync();

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
        /// Метод вернёт список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        public async Task<IEnumerable<BlogOutput>> GetBlogsListAsync()
        {
            try
            {
                var result = await (from b in _postgreDbContext.Blogs
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
        /// Метод вернёт список тем блогов.
        /// </summary>        
        /// <returns>Список тем блогов.</returns>
        public async Task<IEnumerable<BlogThemesOutput>> GetBlogThemesAsync()
        {
            try
            {
                var result = await (from b in _postgreDbContext.BlogThemes
                        select new BlogThemesOutput
                        {
                            Title = b.Title,
                            ThemeCategoryCode = b.ThemeCategoryCode,
                            DateCreated = b.DateCreated
                        })
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
        /// Метод вернёт список новостей упорядоченный по дате создания.
        /// </summary>
        /// <returns>Список новостей упорядоченный по дате создания. </returns>
        public async Task<IEnumerable<NewsOutput>> GetNewsListAsync()
        {
            try
            {
                var result = await (from n in _postgreDbContext.News
                        orderby n.DateCreated descending
                        select new NewsOutput
                        {
                            NewsId = n.NewsId,
                            DateCreated = n.DateCreated,
                            IsMarginTop = n.IsMarginTop,
                            IsPaid = n.IsPaid,
                            Name = n.Name,
                            Text = n.Text,
                            Type = n.Type,
                            Url = n.Url
                        })
                    .ToListAsync();

                // Вычислит поля даты и времени.
                var i = 0;
                var nowDay = DateTime.Now.Day;

                foreach (var item in result)
                {
                    // Первому элементу не нужен отступ.
                    if (i == 0)
                    {
                        item.IsMarginTop = false;
                    }

                    // Если день совпадает с сегодня, то проставит флаг и надпись.
                    if (item.DateCreated.Day == nowDay)
                    {
                        item.IsToday = true;
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
        /// Метод обновит статью.
        /// </summary>
        /// <param name="articleId">Идентифкатор статьи.</param>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <param name="urls">Путь к изображениям.</param>
        /// <param name="title">Название статьи.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="description">Описание статьи.</param>
        /// <param name="text">Полный текст статьи.</param>
        /// <param name="articleCode">Код статьи.</param>
        /// <returns>Данные статьи.</returns>
        public async Task<ArticleOutput> UpdateArticleAsync(long articleId, long blogId, string[] urls, string title,
            int position, string description, string text, Guid articleCode)
        {
            try
            {
                // var getArticle = await _postgreDbContext.Articles
                //     .AsNoTracking()
                //     .Where(d => d.ArticleId == articleId)
                //     .FirstOrDefaultAsync();
                //
                // var articlesUrls = await _commonService.JoinArrayWithDelimeterAsync(urls);
                //
                // var article = new ArticleEntity
                // {
                //     ArticleId = articleId,
                //     BlogId = blogId,
                //     Urls = articlesUrls,
                //     Title = title,
                //     Position = position,
                //     Description = description,
                //     Text = text,
                //     DateCreated = DateTime.Now,
                //     ArticleCode = articleCode
                // };
                //
                // // Обновит статью.
                // if (getArticle != null)
                // {
                //     _postgreDbContext.Articles.Update(article);
                //     await _postgreDbContext.SaveChangesAsync();
                // }
                //
                // var jsonString = JsonConvert.SerializeObject(article);
                // var result = JsonConvert.DeserializeObject<ArticleOutput>(jsonString);
                //
                // return result;
                throw new NotImplementedException();
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
        /// Обновит существующий блог.
        /// </summary>
        /// <param name="blogId">Идентификатор обновляемого блога.</param>
        /// <param name="title">Название блога.</param>
        /// <param name="url">Путь к файлу.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="blogThemeId">Идентификатор темы блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> UpdateBlogAsync(long blogId, string title, string url, bool isPaid, int position,
            string blogThemeId)
        {
            try
            {
                var getBlog = await _postgreDbContext.Blogs
                    .AsNoTracking()
                    .Where(d => d.BlogId == blogId)
                    .FirstOrDefaultAsync();
                
                if (getBlog == null)
                {
                    throw new NotFoundBlogException(blogId);
                }

                var blog = new BlogEntity
                {
                    BlogId = blogId,
                    Title = title,
                    Url = url,
                    Position = position,
                    IsPaid = isPaid,
                    ThemeCategoryCode = blogThemeId,
                    DateCreated = DateTime.Now
                };
                
                _postgreDbContext.Blogs.Update(blog);
                await _postgreDbContext.SaveChangesAsync();

                var jsonString = JsonConvert.SerializeObject(blog);
                var result = JsonConvert.DeserializeObject<BlogOutput>(jsonString);

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
        /// <param name="newsId">Идентификатор новости.</param>
        /// <param name="name">Название новости.</param>
        /// <param name="text">Текст новости.</param>
        /// <param name="url">Путь к изображению.</param>
        /// <param name="isToday">Создана ли сегодня.</param>
        /// <param name="type">Тип статьи.</param>
        /// <param name="isMarginTop">Нужен ли отступ сверху.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <returns>Данные новости.</returns>
        public async Task<NewsOutput> UpdateNewsAsync(long newsId, string name, string text, string url, bool isToday,
            string type, bool isMarginTop, bool isPaid)
        {
            try
            {
                var getNews = await _postgreDbContext.News
                    .AsNoTracking()
                    .Where(d => d.NewsId == newsId)
                    .FirstOrDefaultAsync();

                var news = new NewsEntity
                {
                    NewsId = newsId,
                    Name = name,
                    Text = text,
                    Url = url,
                    IsToday = isToday,
                    Type = type,
                    IsMarginTop = isMarginTop,
                    IsPaid = isPaid,
                    DateCreated = DateTime.Now
                };

                //TODO: обработать ситуацию, если такой новости не найдено.
                // Обновит новость.
                if (getNews != null)
                {
                    _postgreDbContext.News.Update(news);
                    await _postgreDbContext.SaveChangesAsync();
                }

                var jsonString = JsonConvert.SerializeObject(news);
                var result = JsonConvert.DeserializeObject<NewsOutput>(jsonString);

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
        /// Метод получит список тем для статей блогов.
        /// </summary>
        /// <returns>Список тем.</returns>
        public async Task<IEnumerable<ArticleThemeOutput>> GetArticleThemesAsync()
        {
            try
            {
                var result = await _postgreDbContext.ArticleThemes
                    .Select(s => new ArticleThemeOutput
                    {
                        ThemeCode = s.ThemeCode,
                        Position = s.Position,
                        ThemeId = s.ThemeId,
                        ThemeName = s.ThemeName
                    })
                    .OrderBy(o => o.Position)
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
        /// Метод получит блог по его Id.
        /// </summary>
        /// <param name="blogId">Id блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogEntity> GetBlogByIdAsync(long blogId)
        {
            try
            {
                var result = await _postgreDbContext.Blogs
                    .Where(b => b.BlogId == blogId)
                    .FirstOrDefaultAsync();

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
        public async Task<ArticleEntity> GetBlogArticleByUdAsync(long articleId)
        {
            try
            {
                var result = await _postgreDbContext.Articles.Where(a => a.ArticleId == articleId)
                    .FirstOrDefaultAsync();

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
    }
}