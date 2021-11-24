using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Chat
{
    /// <summary>
    /// Класс сопоставляется с таблицей участников диалога Communications.DialogMembers.
    /// </summary>
    [Table("DialogMembers", Schema = "Communications")]
    public sealed class DialogMemberEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long MemberId { get; set; }

        /// <summary>
        /// Id участника диалога.
        /// </summary>
        [Column("UserId")]
        public string Id { get; set; }

        /// <summary>
        /// Дата и время присоединения к диалогу.
        /// </summary>
        [Column("Joined", TypeName = "timestamp")]
        public DateTime Joined { get; set; }

        /// <summary>
        /// Id диалога.
        /// </summary>
        public long DialogId { get; set; }

        [ForeignKey("Id")]
        public UserEntity User { get; set; }

        [ForeignKey("DialogId")]
        public MainInfoDialogEntity Dialog { get; set; }
    }
}
