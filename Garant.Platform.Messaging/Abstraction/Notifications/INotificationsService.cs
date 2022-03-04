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

        /// <summary>
        /// Метод отправит уведомление об успешном создании карточки и отправки ее на модерацию.
        /// </summary>
        Task SendCardModerationAsync();

        /// <summary>
        /// Метод отправит уведомление о незаполненности обязательных полей при создании сферы или категории в конфигураторе.
        /// </summary>
        Task SendErrorMessageCreateSphereCategoryAsync();
    }
}