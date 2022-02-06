using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.News
{
    /// <summary>
    /// Класс сопоставляется с таблицей Info.News.
    /// </summary>
    [Table("News", Schema = "Info")]
    public class NewsEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long NewsId { get; set; }

        /// <summary>
        /// Заголовок новости.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Основной текст новости.
        /// </summary>
        [Column("Text", TypeName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Путь иконки новости.
        /// </summary>
        [Column("Url", TypeName = "text")]
        public string Url { get; set; }

        /// <summary>
        /// Дата и время создания новости.
        /// </summary>
        [Column("DateCreated", TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Тематика новости.
        /// </summary>
        [Column("Type", TypeName = "varchar(100)")]
        public string Type { get; set; }

        /// <summary>
        /// Флаг оплаты размещения новости в топе.
        /// </summary>
        [Column("IsPaid", TypeName = "bool")]
        public bool IsPaid { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}
