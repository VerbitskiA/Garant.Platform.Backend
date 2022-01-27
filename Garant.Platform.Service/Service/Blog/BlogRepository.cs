using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Blog.Input;
using Garant.Platform.Models.Blog.Output;
using Garant.Platform.Models.Entities.Blog;
using Garant.Platform.Models.Entities.News;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garant.Platform.Services.Service.Blog
{
    /// <summary>
    /// Репозиторий блогов.
    /// </summary>
    public class BlogRepository : IBlogRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public BlogRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Создаст новый блог.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <param name="url">Путь к файлу.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="blogThemeId">Идентификатор темы блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> CreateBlogAsync(string title, string url, bool isPaid, int position, long blogThemeId)
        {
            try
            {
                var blog = new BlogEntity
                {
                    Title = title,
                    Url = url,
                    Position = position,
                    IsPaid = isPaid,
                    BlogThemeId = blogThemeId,
                    DateCreated = DateTime.Now
                };
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

        public async Task<NewsOutput> CreateNewsAsync(string name, string text, string url, bool isToday, string type, bool isMarginTop, bool isPaid)
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
                        Title = b.Title,
                        Url = b.Url,
                        IsPaid = b.IsPaid,
                        Position = b.Position,
                        DateCreated = b.DateCreated,
                        BlogThemeId = b.BlogThemeId
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
                                        Title = b.Title,
                                        Url = b.Url,
                                        IsPaid = b.IsPaid,
                                        Position = b.Position,
                                        DateCreated = b.DateCreated,
                                        BlogThemeId = b.BlogThemeId
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
        /// Обновит существующий блог.
        /// </summary>
        /// <param name="blogId">Идентификатор обновляемого блога.</param>
        /// <param name="title">Название блога.</param>
        /// <param name="url">Путь к файлу.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="blogThemeId">Идентификатор темы блога.</param>
        /// <returns>Данные блога.</returns>
        public async Task<BlogOutput> UpdateBlogAsync(long blogId, string title, string url, bool isPaid, int position, long blogThemeId)
        {
            try
            {
                var getBlog = await _postgreDbContext.Blogs
                    .AsNoTracking()
                    .Where(d => d.BlogId == blogId)
                    .FirstOrDefaultAsync();

                var blog = new BlogEntity
                {
                    BlogId = blogId,
                    Title = title,
                    Url = url,
                    Position = position,
                    IsPaid = isPaid,
                    BlogThemeId = blogThemeId,
                    DateCreated = DateTime.Now
                };

                //TODO: обработать ситуацию, если такого блога не найдено.
                // Обновит блог.
                if (getBlog != null)
                {
                    getBlog.BlogId = blog.BlogId;

                    _postgreDbContext.Blogs.Update(blog);
                    await _postgreDbContext.SaveChangesAsync();
                }

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

        public async Task<NewsOutput> UpdateNewsAsync(long newsId, string name, string text, string url, bool isToday, string type, bool isMarginTop, bool isPaid)
        {
            try
            {
                var getNews = await _postgreDbContext.News
                    .AsNoTracking()
                    .Where(d => d.NewsId == newsId)
                    .FirstOrDefaultAsync();

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

                //TODO: обработать ситуацию, если такой новости не найдено.
                // Обновит новость.
                if (getNews != null)
                {
                    getNews.NewsId = news.NewsId;

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
    }
}
