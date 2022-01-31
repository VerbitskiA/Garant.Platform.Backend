using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Blog.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Blog
{
    /// <summary>
    /// Абстракция сервиса блогов и новостей.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// Метод вернёт список новостей упорядоченный по дате создания.
        /// </summary>
        /// <returns>Список новостей упорядоченный по дате создания.</returns>
        Task<IEnumerable<NewsOutput>> GetNewsListAsync();

        /// <summary>
        /// Метод вернёт список статей, относящихся к блогу, упорядоченный по дате создания.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns>Список статей блога упорядоченный по дате создания.</returns>
        Task<IEnumerable<ArticleOutput>> GetArticlesFromBlogAsync(long blogId);

        /// <summary>
        /// Метод создаст новость.
        /// </summary>
        /// <param name="newsData">Данные новости.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные новости.</returns>
        Task<NewsOutput> CreateNewsAsync(string newsData, IFormCollection images);

        /// <summary>
        /// Метод обновит новость.
        /// </summary>
        /// <param name="newsData">Данные новости.</param>
        /// <param name="images">Изображения новости.</param>
        /// <returns>Данные новости.</returns>
        Task<NewsOutput> UpdateNewsAsync(string newsData, IFormCollection images);

        /// <summary>
        /// Метод создаст статью.
        /// </summary>
        /// <param name="articleData">Данные статьи.</param>
        /// <param name="images">Изображения статьи.</param>
        /// <returns>Данные статьи.</returns>
        Task<ArticleOutput> CreateArticleAsync(string articleData, IFormCollection images);

        /// <summary>
        /// Метод обновит статью.
        /// </summary>
        /// <param name="articleData">Данные статьи.</param>
        /// <param name="images">Изображения статьи.</param>
        /// <returns>Данные статьи.</returns>
        Task<ArticleOutput> UpdateArticleAsync(string articleData, IFormCollection images);

        /// <summary>
        /// Метод получит список блогов, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        Task<IEnumerable<BlogOutput>> GetBlogsListMainPageAsync();

        /// <summary>
        /// Метод получит список новостей, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список новостей.</returns>
        Task<IEnumerable<NewsOutput>> GetTopNewsMainPageAsync();

        /// <summary>
        /// Метод получит список тем блогов.
        /// </summary>
        /// <returns>Список тем блогов.</returns>
        Task<IEnumerable<BlogThemesOutput>> GetBlogThemesAsync();

        /// <summary>
        /// Метод получит список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        Task<IEnumerable<BlogOutput>> GetBlogsListAsync();

        /// <summary>
        /// Метод создаст блог.
        /// </summary>
        /// <param name="blogData">Входные данные блога.</param>
        /// <param name="images">Файлы изображений.</param>
        /// <returns>Созданный блог.</returns>
        Task<BlogOutput> CreateBlogAsync(string blogData, IFormCollection images);

        /// <summary>
        /// Метод обновит существующий блог.
        /// </summary>
        /// <param name="blogData">Входные данные блога.</param>
        /// <param name="images">Файлы изображений.</param>
        /// <returns>Обновлённый блог.</returns>
        Task<BlogOutput> UpdateBlogAsync(string blogData, IFormCollection images);

        /// <summary>
        /// Метод получит список тем для статей блогов.
        /// </summary>
        /// <returns>Список тем.</returns>
        Task<IEnumerable<ArticleThemeOutput>> GetArticleThemesAsync();
    }
}
