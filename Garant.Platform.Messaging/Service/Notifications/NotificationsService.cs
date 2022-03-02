using System.Threading.Tasks;
using Garant.Platform.Core.Utils;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Core;
using Microsoft.AspNetCore.SignalR;

namespace Garant.Platform.Messaging.Service.Notifications
{
    /// <summary>
    /// Класс реализует методы сервиса уведомлений.
    /// </summary>
    public class NotificationsService : INotificationsService
    {
        private readonly IHubContext<NotifyHub> _hubContext;
        
        public NotificationsService()
        {
            _hubContext = AutoFac.Resolve<IHubContext<NotifyHub>>();
        }

        /// <summary>
        /// Метод отправит уведомление, если не были заполнены данные о пользователе в профиле.
        /// </summary>
        public async Task SendNotifyEmptyUserInfoAsync()
        {
            await _hubContext.Clients.All.SendAsync("SendNotifyEmptyUserInfo", "Не заполнены данные о себе. Перейдите в профиль для их заполнения.");
        }
    }
}