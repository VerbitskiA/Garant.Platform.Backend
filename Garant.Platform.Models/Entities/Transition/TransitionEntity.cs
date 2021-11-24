using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Transition
{
    /// <summary>
    /// Класс сопоставляется с таблицей переходов dbo.Transitions.
    /// </summary>
    [Table("Transitions", Schema = "dbo")]
    public class TransitionEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long TransitionId { get; set; }

        /// <summary>
        /// Id пользователя, который совершил переход.
        /// </summary>
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        /// <summary>
        /// Тип перехода.
        /// </summary>
        [Column("TransitionType", TypeName = "varchar(100)")]
        public string TransitionType { get; set; }

        /// <summary>
        /// Id франшизы или готового бизнеса.
        /// </summary>
        [Column("ReferenceId", TypeName = "bigint")]
        public long ReferenceId { get; set; }

        /// <summary>
        /// Id другого пользователя.
        /// </summary>
        [Column("OtherId", TypeName = "text")]
        public string OtherId { get; set; }

        /// <summary>
        /// Тип обсуждения.
        /// </summary>
        [Column("TypeItem", TypeName = "varchar(100)")]
        public string TypeItem { get; set; }
    }
}
