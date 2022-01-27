using System;

namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели блога.
    /// </summary>
    public class BlogOutput
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Оплачено ли размещение на главной.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Позиция при размещении.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Идентификатор темы блога.
        /// </summary>
        public long BlogThemeId { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
