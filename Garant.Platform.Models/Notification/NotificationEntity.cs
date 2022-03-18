using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Notification
{
    /// <summary>
    /// Класс сопоставляется с таблицей уведомлений.
    /// </summary>
    [Table("Notifications", Schema = "Communications")]
    public class NotificationEntity
    {
        [Key]
        public long NotificationId { get; set; }

        /// <summary>
        /// Тип уведомления.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("NotificationType", TypeName = "varchar(100)")]
        public string NotificationType { get; set; }

        /// <summary>
        /// Заголовок уведомления.
        /// </summary>
        [Required]
        [MaxLength(150)]
        [Column("NotificationTitle", TypeName = "varchar(150)")]
        public string NotificationTitle { get; set; }

        /// <summary>
        /// Текст уведомления.
        /// </summary>
        [Required]
        [MaxLength(400)]
        [Column("NotificationText", TypeName = "varchar(400)")]
        public string NotificationText { get; set; }

        /// <summary>
        /// Дата создания уведомления.
        /// </summary>
        [Required]
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Уровень уведомления.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("NotificationLevel", TypeName = "varchar(100)")]
        public string NotificationLevel { get; set; }

        /// <summary>
        /// Является ли уведомление для пользователя.
        /// Если да, то оно связывается с пользователем.
        /// Если нет, то является общим для системы.
        /// </summary>
        [Required]
        [Column("IsUserNotify", TypeName = "bool")]
        public bool IsUserNotify { get; set; }

        /// <summary>
        /// Id пользователя, с которым связано уведомление.
        /// FK не делать, потому что может быть null.
        /// </summary>
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        /// <summary>
        /// Системное название уведомления.
        /// </summary>
        [Required]
        [MaxLength(300)]
        [Column("ActionNotifySysName", TypeName = "varchar(300)")]
        public string ActionNotifySysName { get; set; }
    }
}