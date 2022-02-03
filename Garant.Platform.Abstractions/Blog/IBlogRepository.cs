using Garant.Platform.Models.Blog.Output;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Entities.Blog;

namespace Garant.Platform.Abstractions.Blog
{
    /// <summary>
    /// Абстракция репозитория блогов и новостей.
    /// </summary>
    public interface IBlogRepository
    {
        /// <summary>
        /// Метод создаст новость.
        /// </summary>
        /// <param name="name">Название новости.</param>
        /// <param name="text">Текст новости.</param>
        /// <param name="url">Путь к изображению.</param>
        /// <param name="isToday">Создана ли сегодня.</param>
        /// <param name="type">Тип статьи.</param>
        /// <param name="isMarginTop">Нужен ли отступ сверху.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <returns>Данные новости.</returns>
        Task<NewsOutput> CreateNewsAsync(string name, string text, string url, bool isToday, string type, bool isMarginTop, bool isPaid);

        /// <summary>
        /// Метод обновит новость.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <param name="name">Название новости.</param>
        /// <param name="text">Текст новости.</param>
        /// <param name="url">Путь к изображению.</param>
        /// <param name="isToday">Создана ли сегодня.</param>
        /// <param name="type">Тип статьи.</param>
        /// <param name="isMarginTop">Нужен ли отступ сверху.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <returns>Данные новости.</returns>
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
        /// Метод вернёт список новостей упорядоченный по дате создания.
        /// </summary>
        /// <returns>Список новостей упорядоченный по дате создания. </returns>
        Task<IEnumerable<NewsOutput>> GetNewsListAsync();

        /// <summary>
        /// Метод вернёт список статей, относящихся к блогу, упорядоченный по дате создания.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <returns>Список статей упорядоченный по дате создания.</returns>
        Task<IEnumerable<ArticleOutput>> GetArticlesFromBlogAsync(long blogId);

        /// <summary>
        /// Метод создаст новый блог.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <param name="url">Путь к изображению.</param>
        /// <param name="blogThemeCode">Код темы блога.</param>        
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> CreateBlogAsync(string title, string url, string blogThemeCode);

        /// <summary>
        /// Метод обновит существующий блог.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <param name="title">Название блога.</param>
        /// <param name="url">Путь к изображению.</param>
        /// <param name="isPaid">Оплачено ли размещение на главной.</param>
        /// <param name="position">Позиция при размежении.</param>
        /// <param name="blogThemeId">Идентификатор темы блога.</param> 
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> UpdateBlogAsync(long blogId, string title, string url, bool isPaid, int position, string blogThemeId);

        /// <summary>
        /// Метод получит блог по названию.
        /// </summary>
        /// <param name="title">Название блога.</param>
        /// <returns>Данные блога.</returns>
        Task<BlogOutput> GetBlogAsync(string title);

        /// <summary>
        /// Метод создаст новую статью в блоге.
        /// </summary>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <param name="previewUrl">Изображение превью.</param>
        /// <param name="articleUrl">Изображение статьи.</param>
        /// <param name="title">Название статьи.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="description">Описание статьи.</param>
        /// <param name="text">Полный текст статьи.</param>
        /// <param name="articleCode">Код статьи.</param>
        /// <param name="signatureText">Подпись.</param>
        /// <returns>Данные статьи.</returns>
        Task<ArticleOutput> CreateArticleAsync(long blogId, string previewUrl, string articleUrl, string title, string description, string text, string articleCode, string signatureText);

        /// <summary>
        /// Метод обновит статью.
        /// </summary>
        /// <param name="articleId">Идентифкатор статьи.</param>
        /// <param name="blogId">Идентификатор блога.</param>
        /// <param name="urls">Путь к изображениям.</param>
        /// <param name="title">Название статьи.</param>
        /// <param name="position">Позиция при размещении.</param>
        /// <param name="description">Описание статьи.</param>
        /// <param name="text">Полный текст статьи.</param>
        /// <param name="articleCode">Код статьи.</param>
        /// <returns>Данные статьи.</returns>
        Task<ArticleOutput> UpdateArticleAsync(long articleId, long blogId, string[] urls, string title, int position, string description, string text, Guid articleCode);
        
        /// <summary>
        /// Метод получит список тем для статей блогов.
        /// </summary>
        /// <returns>Список тем.</returns>
        Task<IEnumerable<ArticleThemeOutput>> GetArticleThemesAsync();

        /// <summary>
        /// Метод получит блог по его Id.
        /// </summary>
        /// <param name="blogId">Id блога.</param>
        /// <returns>Данные блога.</returns>
        Task<BlogEntity> GetBlogByIdAsync(long blogId);
        
        /// <summary>
        /// Метод получит статью блога.
        /// </summary>
        /// <param name="articleId">Id статьи.</param>
        /// <returns>Данные статьи.</returns>
        Task<ArticleEntity> GetBlogArticleByUdAsync(long articleId);
    }
}
