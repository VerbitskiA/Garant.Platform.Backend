using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Blog.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Blog
{
    /// <summary>
    /// Сервис блогов и новостей.
    /// </summary>
    public sealed class BlogService : IBlogService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public BlogService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// /// Метод получит список объявлений для главной страницы.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        public async Task<IEnumerable<BlogOutput>> GetBlogsListMainPageAsync()
        {
            try
            {
                var result = await (from b in _postgreDbContext.Blogs
                                    where b.IsPaid.Equals(true)
                                    select new BlogOutput
                                    {
                                        Title = b.Title,
                                        Url = b.Url
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
                var result = await (from n in _postgreDbContext.News
                                    where n.IsPaid.Equals(true)
                                    select new NewsOutput
                                    {
                                        DateCreated = n.DateCreated,
                                        IsMarginTop = n.IsMarginTop,
                                        IsPaid = n.IsPaid,
                                        Name = n.Name,
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
        /// Метод получит список тем блогов.
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
    }
}
