using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Commerce
{
    /// <summary>
    /// Класс сопоставляется с таблицей сделок Commerce.Deals.
    /// </summary>
    [Table("Deals", Schema = "Commerce")]
    public class DealEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long DealId { get; set; }

        /// <summary>
        /// Id предмета сделки (франшизы или бизнеса).
        /// </summary>
        [Column("DealItemId", TypeName = "bigint")]
        public long DealItemId { get; set; }

        /// <summary>
        /// Id пользователя (не владельца предмета сделки).
        /// </summary>
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        /// <summary>
        /// Флаг завершенности сделки.
        /// </summary>
        [Column("IsCompletedDeal", TypeName = "bool")]
        public bool IsCompletedDeal { get; set; }

        /// <summary>
        /// Дата создания сделки.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Закрыта ли сделка.
        /// </summary>
        [Column("IsClose", TypeName = "bool")]
        public bool IsClose { get; set; }
    }
}
