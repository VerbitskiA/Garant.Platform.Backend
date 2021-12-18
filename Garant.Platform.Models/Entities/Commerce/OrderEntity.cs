using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Commerce
{
    /// <summary>
    /// Класс сопоставляется с таблицей заказов Commerce.Orders.
    /// </summary>
    [Table("Orders", Schema = "Commerce")]
    public class OrderEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        [Column("OrderId", TypeName = "bigserial")]
        public long OrderId { get; set; }

        /// <summary>
        /// Краткое назание заказа.
        /// </summary>
        [Column("ShortOrderName", TypeName = "varchar(200)")]
        public string ShortOrderName { get; set; }

        /// <summary>
        /// Полное назание заказа.
        /// </summary>
        [Column("FullOrderName", TypeName = "varchar(500)")]
        public string FullOrderName { get; set; }

        /// <summary>
        /// Дата создания платежа.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Статус заказа.
        /// </summary>
        [Column("OrderStatus", TypeName = "varchar(100)")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        [Column("Amount", TypeName = "numeric")]
        public double Amount { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        [Column("Currency", TypeName = "varchar(100)")]
        [DefaultValue("RUB")]
        public string Currency { get; set; }

        /// <summary>
        /// Кол-во товара.
        /// </summary>
        [Column("ProductCount", TypeName = "int")]
        public int ProductCount { get; set; }

        /// <summary>
        /// Сумма (всего).
        /// </summary>
        [Column("TotalAmount", TypeName = "numeric")]
        public double TotalAmount { get; set; }

        /// <summary>
        /// Список названий документов приложенных к заказу разделенные через запятую.
        /// </summary>
        [Column("DocumentsNames", TypeName = "varchar(500)")]
        public string DocumentsNames { get; set; }

        /// <summary>
        /// Тип заказа (франшиза или бизнес).
        /// </summary>
        [Column("OrderType", TypeName = "varchar(100)")]
        public string OrderType { get; set; }

        /// <summary>
        /// Id исходного товара (Id франшизы или бизнеса).
        /// </summary>
        [Column("OriginalId", TypeName = "bigint")]
        public long OriginalId { get; set; }

        /// <summary>
        /// Флаг завершенности заказа.
        /// </summary>
        [Column("IsCompleted", TypeName = "bool")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Дата завершения заказа.
        /// </summary>
        [Column("DateCompleted", TypeName = "timestamp")]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Флаг отмены заказа.
        /// </summary>
        [Column("IsCancel", TypeName = "bool")]
        public bool IsCancel { get; set; }

        /// <summary>
        /// Дата отмены заказа.
        /// </summary>
        [Column("DateCanceled", TypeName = "timestamp")]
        public DateTime? DateCanceled { get; set; }

        /// <summary>
        /// Id пользователя, который создал заказ.
        /// </summary>
        [ForeignKey("Id")]
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
