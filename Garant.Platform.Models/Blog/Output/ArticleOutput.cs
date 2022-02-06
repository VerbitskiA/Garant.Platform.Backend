using System;

namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели статьи.
    /// </summary>
    public class ArticleOutput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// FK, идентификатор блога.
        /// </summary>
        public long BlogId { get; set; }        

        /// <summary>
        /// Изображение превью облажки.
        /// </summary>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Изображение статьи.
        /// </summary>
        public string ArticleUrl { get; set; }

        /// <summary>
        /// Заголовок статьи.
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
        /// Позиция при размещении.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime DateCreated { get; set; }
        
        /// <summary>
        /// Код статьи.
        /// </summary>
        public string ArticleCode { get; set; }

        /// <summary>
        /// Код темы статьи. 
        /// </summary>
        public string ThemeCode { get; set; }

        /// <summary>
        /// Подпись основного изображения статьи.
        /// </summary>
        public string SignatureText { get; set; }
    }
}
