using Garant.Platform.Models.Blog.Input;
using Garant.Platform.Models.Blog.Output;
using Garant.Platform.Models.Entities.Blog;
using System.Collections.Generic;
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

        /// <summary>
        /// Метод вернёт список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        Task<IEnumerable<BlogOutput>> GetBlogsListAsync();

        /// <summary>
        /// Метод создаст новый блог.
        /// </summary>
        /// <param name="blogInput">Входная модель.</param>
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> CreateBlog(CreateBlogInput blogInput);

        /// <summary>
        /// Метод обновит существующий блог
        /// </summary>
        /// <param name="blogInput"></param>
        /// <returns></returns>
        Task<BlogOutput> UpdateBlog(UpdateBlogInput blogInput);

        /// <summary>
        /// Метод получит блог по названию.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> GetBlogAsync(string title);
    }
}
