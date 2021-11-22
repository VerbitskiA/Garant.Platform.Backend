using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Chat
{
    /// <summary>
    /// Класс сопоставляется с таблицей сообщений Communications.DialogMessages.
    /// </summary>
    [Table("DialogMessages", Schema = "Communications")]
    public sealed class DialogMessageEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long MessageId { get; set; }

        /// <summary>
        /// Id диалога.
        /// </summary>
        public long DialogId { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        [Column("Message", TypeName = "text")]
        public string Message { get; set; }

        /// <summary>
        /// Дата и время сообщения.
        /// </summary>
        [Column("Created", TypeName = "timestamp")]
        public DateTime Created { get; set; }

        [ForeignKey("DialogId")]
        public MainInfoDialogEntity Dialog { get; set; }

        /// <summary>
        /// Id пользователя, которому принадлежит сообщение.
        /// </summary>        
        public string UserId { get; set; }

        /// <summary>
        /// Флаг принадлежности сообщения текущему пользователю.
        /// </summary>
        [ForeignKey("IsMyMessage")]
        public bool IsMyMessage { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}
