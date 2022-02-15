using System.Threading.Tasks;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Models.Notification.Output;

namespace Garant.Platform.Messaging.Service.Notifications
{
    /// <summary>
    /// Класс реализует
    /// </summary>
    public class NotificationsService : INotificationsService
    {
        public NotificationsService()
        {
        }

        /// <summary>
        /// Метод запишет уведомление после создания события.
        /// </summary>
        /// <param name="actionNotifySysName">Системное название события.</param>
        /// <returns>Данные уведомления.</returns>
        public Task<NotificationOutput> SetAfterActionNotificationAsync(string actionNotifySysName)
        {
            throw new System.NotImplementedException();
        }
    }
}