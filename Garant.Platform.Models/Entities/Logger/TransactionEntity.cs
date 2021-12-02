using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Logger
{
    /// <summary>
    /// Класс сопоставляется с таблицей транзакций Logs.Transactions.
    /// </summary>
    [Table("Transactions", Schema = "Logs")]
    public class TransactionEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        [Column("TransactionId", TypeName = "bigserial")]
        public long TransactionId { get; set; }

        /// <summary>
        /// Описание транзакции.
        /// </summary>
        [Column("TransactionText", TypeName = "varchar(500)")]
        public string TransactionText { get; set; }

        /// <summary>
        /// Дата создания транзакции.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Тип транзакции.
        /// </summary>
        [Column("TransactionType", TypeName = "varchar(200)")]
        public string TransactionType { get; set; }

        /// <summary>
        /// Id пользователя, под которым логирована транзакция.
        /// </summary>
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }
    }
}
