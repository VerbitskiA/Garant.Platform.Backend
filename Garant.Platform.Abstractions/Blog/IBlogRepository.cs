using Garant.Platform.Models.Blog.Output;
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
        /// Метод создаст новость.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="url"></param>
        /// <param name="isToday"></param>
        /// <param name="type"></param>
        /// <param name="isMarginTop"></param>
        /// <param name="isPaid"></param>
        /// <returns></returns>
        Task<NewsOutput> CreateNewsAsync(string name, string text, string url, bool isToday, string type, bool isMarginTop, bool isPaid);

        /// <summary>
        /// Метод обновит новость
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="url"></param>
        /// <param name="isToday"></param>
        /// <param name="type"></param>
        /// <param name="isMarginTop"></param>
        /// <param name="isPaid"></param>
        /// <returns></returns>
        Task<NewsOutput> UpdateNewsAsync(long newsId, string name, string text, string url, bool isToday, string type, bool isMarginTop, bool isPaid);

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
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="isPaid"></param>
        /// <param name="position"></param>
        /// <param name="blogThemeId"></param>
        /// <param name="dateCreated"></param>
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> CreateBlogAsync(string title, string url, bool isPaid, int position, long blogThemeId);

        /// <summary>
        /// Метод обновит существующий блог.
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="isPaid"></param>
        /// <param name="position"></param>
        /// <param name="blogThemeId"></param>
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> UpdateBlogAsync(long blogId, string title, string url, bool isPaid, int position, long blogThemeId);

        /// <summary>
        /// Метод получит блог по названию.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> GetBlogAsync(string title);
    }
}
