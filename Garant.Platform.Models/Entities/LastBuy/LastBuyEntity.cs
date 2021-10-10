using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.LastBuy
{
    /// <summary>
    /// Класс сопоставляется с таблицей последних покупок Commerce.LastBuy.
    /// </summary>
    [Table("LastBuy", Schema = "Commerce")]
    public class LastBuyEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Путь
        /// </summary>
        [Column("Url", TypeName = "text")]
        public string Url { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [Column("Name", TypeName = "varchar(200)")]
        public string Name { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        [Column("Text", TypeName = "varchar(400)")]
        public string Text { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        [Column("TextDoPrice", TypeName = "varchar(100)")]
        public string TextDoPrice { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        [Column("Price", TypeName = "numeric")]
        public double Price { get; set; }

        /// <summary>
        /// Дата покупки.
        /// </summary>
        [Column("DateBuy", TypeName = "timestamp")]
        public DateTime DateBuy { get; set; }

        /// <summary>
        /// Кол-во дней.
        /// </summary>
        [Column("CountDays", TypeName = "int")]
        public int CountDays { get; set; }

        /// <summary>
        /// Склонение дней.
        /// </summary>
        [Column("DayDeclination", TypeName = "varchar(10)")]
        public string DayDeclination { get; set; }
    }
}
