using System.ComponentModel.DataAnnotations;

namespace Garant.Platform.Messaging.Models.Notification.Output
{
    /// <summary>
    /// Класс выходной модели уведомлений.
    /// </summary>
    public class NotificationOutput
    {
        [Key]
        public long NotificationId { get; set; }

        /// <summary>
        /// Тип уведомления.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NotificationType { get; set; }

        /// <summary>
        /// Заголовок уведомления.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string NotificationTitle { get; set; }

        /// <summary>
        /// Текст уведомления.
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string NotificationText { get; set; }

        /// <summary>
        /// Дата создания уведомления.
        /// </summary>
        [Required]
        public string DateCreate { get; set; }

        /// <summary>
        /// Уровень уведомления.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NotificationLevel { get; set; }

        /// <summary>
        /// Является ли уведомление для пользователя.
        /// Если да, то оно связывается с пользователем.
        /// Если нет, то является общим для системы.
        /// </summary>
        [Required]
        public bool IsUserNotify { get; set; }

        /// <summary>
        /// Id пользователя, с которым связано уведомление.
        /// FK не делать, потому что может быть null.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Системное название уведомления.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string ActionNotifySysName { get; set; }
    }
}