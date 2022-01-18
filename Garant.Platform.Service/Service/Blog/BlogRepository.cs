using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Blog.Output;
using Microsoft.EntityFrameworkCore;
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
    }
}
