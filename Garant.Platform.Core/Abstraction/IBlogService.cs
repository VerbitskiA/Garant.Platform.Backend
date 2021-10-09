using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Blog.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция сервиса блогов.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// Метод получит список объявлений.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        Task<IEnumerable<BlogOutput>> GetBlogsListAsync();
    }
}
