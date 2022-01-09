using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Commerce
{
    /// <summary>
    /// Класс сопоставляется с таблицей платежей Commerce.Payments.
    /// </summary>
    [Table("Payments", Schema = "Commerce")]
    public class PaymentEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        [Column("PaymentId", TypeName = "bigserial")]
        public long PaymentId { get; set; }

        /// <summary>
        /// Номер расчетного счета плательщика.
        /// </summary>
        [Required]
        [MinLength(20)]
        [MaxLength(20)]
        [Column("AccountNumberPayer", TypeName = "int")]
        public int AccountNumberPayer { get; set; }

        /// <summary>
        /// Наименование получателя.
        /// </summary>
        [Required]
        [Column("RecipientName", TypeName = "varchar(400)")]
        public string RecipientName { get; set; }

        /// <summary>
        /// ИНН получателя. Если при платеже физ.лицу ИНН неизвестен, то ставить 0. Во всех других случаях он необходим.
        /// </summary>
        [DefaultValue(0)]
        [Required]
        [MaxLength(12)]
        [Column("RecipientInn", TypeName = "int")]
        public int RecipientInn { get; set; }

        /// <summary>
        /// КПП получателя.
        /// </summary>
        [MaxLength(9)]
        [Column("RecipientKpp", TypeName = "int")]
        public int? RecipientKpp { get; set; }

        /// <summary>
        /// БИК получателя.
        /// </summary>
        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        [Column("RecipientBik", TypeName = "int")]
        public int RecipientBik { get; set; }

        /// <summary>
        /// Название банка получателя.
        /// </summary>
        [Required]
        [Column("BankName", TypeName = "varchar(400)")]
        public string BankName { get; set; }

        /// <summary>
        /// Корреспондентский счёт банка получателя.
        /// </summary>
        [Required]
        [MinLength(20)]
        [MaxLength(20)]
        [Column("CorrAccountNumber", TypeName = "int")]
        public int CorrAccountNumber { get; set; }

        /// <summary>
        /// Номер расчетного счета получателя.
        /// </summary>
        [Required]
        [MinLength(20)]
        [MaxLength(20)]
        [Column("RecipientAccountNumber", TypeName = "int")]
        public int RecipientAccountNumber { get; set; }

        /// <summary>
        /// Назначение платежа.
        /// </summary>
        [Required]
        [Column("Purpose", TypeName = "text")]
        public string Purpose { get; set; }

        /// <summary>
        /// Сумма платежа в руб.
        /// </summary>
        [Required]
        [Column("Amount", TypeName = "numeric")]
        public double Amount { get; set; }

        /// <summary>
        /// Удержанная сумма в руб.
        /// </summary>
        [Column("CollectionAmount", TypeName = "numeric")]
        public double? CollectionAmount { get; set; }
    }
}
