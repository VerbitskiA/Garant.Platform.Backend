using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Commerce
{
    /// <summary>
    /// Класс сопоставляется с таблицей этапов сделки Commerce.DealIterations.
    /// </summary>
    [Table("DealIterations", Schema = "Commerce")]
    public class DealIterationEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        [Column("IterationId", TypeName = "bigserial")]
        public long IterationId { get; set; }

        /// <summary>
        /// FK на сделку.
        /// </summary>
        [ForeignKey("DealId")]
        [Column("DealIteration", TypeName = "bigint")]
        public long DealIteration { get; set; }

        public DealEntity Deal { get; set; }

        /// <summary>
        /// Номер этапа.
        /// </summary>
        [Column("NumberIteration", TypeName = "int")]
        public int NumberIteration { get; set; }

        /// <summary>
        /// Фоаг завершен ли этап.
        /// </summary>
        [Column("IsCompletedIteration", TypeName = "bool")]
        public bool IsCompletedIteration { get; set; }

        /// <summary>
        /// Название этапа.
        /// </summary>
        [Column("IterationName", TypeName = "varchar(200)")]
        public string IterationName { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }

        /// <summary>
        /// Описание этапа.
        /// </summary>
        [Column("IterationDetail", TypeName = "varchar(150)")]
        public string IterationDetail { get; set; }
    }
}
