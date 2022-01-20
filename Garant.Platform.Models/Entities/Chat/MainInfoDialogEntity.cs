using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Chat
{
    /// <summary>
    /// Класс сопоставляется с таблицей информации о диалогах Communications.MainInfoDialogs.
    /// </summary>
    [Table("MainInfoDialogs", Schema = "Communications")]
    public sealed class MainInfoDialogEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long DialogId { get; set; }

        /// <summary>
        /// Название диалога.
        /// </summary>
        [Column("DialogName", TypeName = "varchar(300)")]
        public string DialogName { get; set; }

        /// <summary>
        /// Дата и время создания диалога.
        /// </summary>
        [Column("Created", TypeName = "timestamp")]
        public DateTime Created { get; set; }
    }
}
