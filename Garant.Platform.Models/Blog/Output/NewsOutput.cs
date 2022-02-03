using System;

namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели новостей.
    /// </summary>
    public class NewsOutput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long NewsId { get; set; }

        /// <summary>
        /// Название новости.
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
        /// Тип.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Флаг оплаты размещения новости на главной странице.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }

        public string Date { get; set; }
        
        public string Time { get; set; }
    }
}
