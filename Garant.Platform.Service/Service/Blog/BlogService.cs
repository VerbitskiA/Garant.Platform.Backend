using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
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
        
        public BlogService(PostgreDbContext postgreDbContext, IBlogRepository blogRepository, IFtpService ftpService)
        {
            _postgreDbContext = postgreDbContext;
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
                        result = await _blogRepository.CreateBlog(blogInput);
                    }
                }

                if (result != null)
                {
                    // Загрузит документы на сервер.
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
        public Task<BlogOutput> UpdateBlogAsync(string blogData, IFormCollection images)
        {
            throw new NotImplementedException();
        }
    }
}
