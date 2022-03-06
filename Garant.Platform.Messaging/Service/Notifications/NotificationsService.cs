using System.Threading.Tasks;
using Garant.Platform.Core.Utils;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Consts;
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
            await _hubContext.Clients.All.SendAsync("SendNotifyEmptyUserInfo", NotifyMessage.NOTIFY_EMPTY_USER_INFO);
        }

        /// <summary>
        /// Метод отправит уведомление об успешном создании карточки и отправки ее на модерацию.
        /// </summary>
        public async Task SendCardModerationAsync()
        {
            await _hubContext.Clients.All.SendAsync("SendCardModeration", NotifyMessage.NOTIFY_EMPTY_USER_INFO);
        }

        /// <summary>
        /// Метод отправит уведомление о незаполненности обязательных полей при создании сферы или категории в конфигураторе.
        /// </summary>
        public async Task SendErrorMessageCreateSphereCategoryAsync()
        {
            await _hubContext.Clients.All.SendAsync("SendEmptySphereCategory", NotifyMessage.ERROR_EMPTY_SPHERE_CATEGORY);
        }
        
        /// <summary>
        /// Метод отправит уведомление о созданной сфере.
        /// </summary>
        public async Task SendCreateSphereAsync()
        {
            await _hubContext.Clients.All.SendAsync("SendCreateSphere", NotifyMessage.SUCCESS_CREATE_SPHERE);
        }
        
        /// <summary>
        /// Метод отправит уведомление о созданной категории.
        /// </summary>
        public async Task SendCreateCategoryAsync()
        {
            await _hubContext.Clients.All.SendAsync("SendCreateCategory", NotifyMessage.SUCCESS_CREATE_CATEGORY);
        }
    }
}