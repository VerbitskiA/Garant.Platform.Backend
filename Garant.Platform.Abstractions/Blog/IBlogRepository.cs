using Garant.Platform.Models.Blog.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garant.Platform.Abstractions.Blog
{
    /// <summary>
    /// Абстракция репозитория блогов.
    /// </summary>
    public interface IBlogRepository
    {
        /// <summary>
        /// Метод вернёт список тем блогов.
        /// </summary>        
        /// <returns>Список тем блогов.</returns>
        Task<IEnumerable<BlogThemesOutput>> GetBlogThemesAsync();
    }
}
