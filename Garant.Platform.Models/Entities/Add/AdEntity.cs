using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Add
{
    /// <summary>
    /// Класс сопоставляется с таблицей объявлений dbo.Ads.
    /// </summary>
    [Table("Ads", Schema = "Info")]
    public class AdEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long AdId { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        [Column("Url", TypeName = "text")]
        public string Url { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        [Column("Text", TypeName = "varchar(400)")]
        public string Text { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        [Column("Price", TypeName = "numeric")]
        public double Price { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        [Column("TextDoPrice", TypeName = "varchar(100)")]
        public string TextDoPrice { get; set; }

        /// <summary>
        /// Кол-во дней.
        /// </summary>
        [Column("CountDays", TypeName = "int")]
        public int CountDays { get; set; }

        /// <summary>
        /// Склонение дней.
        /// </summary>
        [Column("DayDeclination", TypeName = "varchar(100)")]
        public string DayDeclination { get; set; }
    }
}
