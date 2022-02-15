using System.ComponentModel.DataAnnotations;

namespace Garant.Platform.Messaging.Models.Notification.Input
{
    /// <summary>
    /// Класс входной модели уведомлений.
    /// </summary>
    public class NotificationInput
    {
        /// <summary>
        /// Системное название уведомления.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string ActionNotifySysName { get; set; }
    }
}