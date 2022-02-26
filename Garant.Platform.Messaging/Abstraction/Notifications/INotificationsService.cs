using System.Threading.Tasks;
using Garant.Platform.Messaging.Models.Notification.Output;

namespace Garant.Platform.Messaging.Abstraction.Notifications
{
    /// <summary>
    /// Абстракция сервиса уведомлений.
    /// </summary>
    public interface INotificationsService
    {
        /// <summary>
        /// Метод запишет уведомление после создания события.
        /// </summary>
        /// <param name="actionNotifySysName">Системное название события.</param>
        /// <returns>Данные уведомления.</returns>
        Task<NotificationOutput> SetAfterActionNotificationAsync(string actionNotifySysName);
    }
}