using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Blog.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Service.Service.Blog
{
    public sealed class BlogService : IBlogService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public BlogService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод получит список объявлений.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        public async Task<IEnumerable<BlogOutput>> GetBlogsListAsync()
        {
            try
            {
                var result = await (from b in _postgreDbContext.Blogs
                                    select new BlogOutput
                                    {
                                        Title = b.Title,
                                        Url = b.Url
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
