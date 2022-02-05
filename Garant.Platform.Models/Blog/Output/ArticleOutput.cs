using System;

namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели статьи.
    /// </summary>
    public class ArticleOutput
    {
        /// <summary>
        /// Идентификатор статьи.
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// идентификатор блога.
        /// </summary>
        public long  BlogId { get; set; }

        /// <summary>
        /// Путь к изображениям.
        /// </summary>
        public string Urls { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Описание статьи.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Текст статьи.
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Дата создания статьи.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Позиция при размещении.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Код статьи.
        /// </summary>
        public Guid ArticleCode { get; set; }
    }
}
