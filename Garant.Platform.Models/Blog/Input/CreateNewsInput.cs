using System;

namespace Garant.Platform.Models.Blog.Input
{
    public class CreateNewsInput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long NewsId { get; set; }

        /// <summary>
        /// Заголовок новости.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Основной текст новости.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Путь иконки новости.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Дата и время создания новости.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Тематика новости.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Флаг оплаты размещения новости в топе.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }
    }
}
