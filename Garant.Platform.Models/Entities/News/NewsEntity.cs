using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.News
{
    /// <summary>
    /// Класс сопоставляется с таблицей dbo.News.
    /// </summary>
    [Table("News", Schema = "dbo")]
    public class NewsEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long NewsId { get; set; }

        /// <summary>
        /// Название новости.
        /// </summary>
        [Column("Name", TypeName = "varchar(300)")]
        public string Name { get; set; }

        /// <summary>
        /// Путь иконки новости.
        /// </summary>
        [Column("IconUrl", TypeName = "text")]
        public string IconUrl { get; set; }

        /// <summary>
        /// Дата и время создания новости.
        /// </summary>
        [Column("FullDateCreated", TypeName = "timestamp")]
        public DateTime FullDateCreated { get; set; }

        /// <summary>
        /// Флаг отобразить ли надпись сегодня вместо даты.
        /// </summary>
        [Column("IsToday", TypeName = "bool")]
        public bool IsToday { get; set; }

        /// <summary>
        /// Время создания новости.
        /// </summary>
        [Column("TimeCreated", TypeName = "time ")]
        public string TimeCreated { get; set; }

        /// <summary>
        /// Дата создания новости.
        /// </summary>
        [Column("DateCreated", TypeName = "time ")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        [Column("Type", TypeName = "varchar(200)")]
        public string Type { get; set; }

        /// <summary>
        /// Флаг применить ли отступ снизу от новости.
        /// </summary>
        [Column("IsMarginBottom", TypeName = "bool")]
        public bool IsMarginBottom { get; set; }
    }
}
