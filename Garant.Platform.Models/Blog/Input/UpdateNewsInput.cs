
using System;

namespace Garant.Platform.Models.Blog.Input
{
    public class UpdateNewsInput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long NewsId { get; set; }
        
        /// <summary>
        /// Название новости
        /// </summary>
        public string Name { get; set; }

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
        /// Флаг отобразить ли надпись сегодня вместо даты.
        /// </summary>
        public bool IsToday { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Флаг применить ли отступ сверху от новости.
        /// </summary>
        public bool IsMarginTop { get; set; }

        /// <summary>
        /// Флаг оплаты размещения новости на главной странице.
        /// </summary>
        public bool IsPaid { get; set; }
    }
}
