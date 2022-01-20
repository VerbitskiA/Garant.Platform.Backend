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
        /// Название новости.
        /// </summary>
        [Column("Name", TypeName = "varchar(200)")]
        public string Name { get; set; }

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
        /// Флаг отобразить ли надпись сегодня вместо даты.
        /// </summary>
        [Column("IsToday", TypeName = "bool")]
        public bool IsToday { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        [Column("Type", TypeName = "varchar(100)")]
        public string Type { get; set; }

        /// <summary>
        /// Флаг применить ли отступ снизу от новости.
        /// </summary>
        [Column("IsMarginTop", TypeName = "bool")]
        public bool IsMarginTop { get; set; }

        /// <summary>
        /// Флаг оплаты размещения новости на главной странице.
        /// </summary>
        [Column("IsPaid", TypeName = "bool")]
        public bool IsPaid { get; set; }
    }
}
