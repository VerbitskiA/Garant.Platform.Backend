using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей Franchises.Franchises.
    /// </summary>
    [Table("Franchises", Schema = "Franchises")]
    public class FranchiseEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long FranchiseId { get; set; }

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

        /// <summary>
        /// Категория франшизы.
        /// </summary>
        [Column("Category", TypeName = "varchar(100)")]
        public string Category { get; set; }

        /// <summary>
        /// Подкатегория.
        /// </summary>
        [Column("SubCategory", TypeName = "text")]
        public string SubCategory { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        [Column("ViewBusiness", TypeName = "varchar(200)")]
        public string ViewBusiness { get; set; }

        /// <summary>
        /// Покупка через гарант.
        /// </summary>
        [Column("IsGarant", TypeName = "bool")]
        public bool IsGarant { get; set; }

        [Column("City", TypeName = "varchar(200)")]
        public string City { get; set; }

        /// <summary>
        /// Желаемая прибыль в мес.
        /// </summary>
        [Column("ProfitPrice", TypeName = "numeric")]
        public double ProfitPrice { get; set; }
    }
}
