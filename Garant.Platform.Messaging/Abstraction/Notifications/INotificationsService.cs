using System.Threading.Tasks;

namespace Garant.Platform.Messaging.Abstraction.Notifications
{
    /// <summary>
    /// Абстракция сервиса уведомлений.
    /// </summary>
    public interface INotificationsService
    {
        /// <summary>
        /// Метод отправит уведомление, если не были заполнены данные о пользователе в профиле.
        /// </summary>
        Task SendNotifyEmptyUserInfoAsync();
    }
}