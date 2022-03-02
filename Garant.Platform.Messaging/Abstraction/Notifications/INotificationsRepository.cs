using System.Threading.Tasks;

namespace Garant.Platform.Messaging.Abstraction.Notifications
{
    /// <summary>
    /// Абстракция репозитория уведомлений для работы с БД.
    /// </summary>
    public interface INotificationsRepository
    {
        /// <summary>
        /// Метод запишет уведомление в БД.
        /// </summary>
        /// <param name="notifyType">Тип уведомления.</param>
        /// <param name="notifyTitle">Заголовок уведомления.</param>
        /// <param name="notifyText">Текст уведомления.</param>
        /// <param name="notifyLvl">Уровень уведомления.</param>
        /// <param name="isUserNotify">Принадлежит ли уведомление пользователю.</param>
        /// <param name="userId">Id ользователя.</param>
        /// <param name="notifySysName">Системное название уведомления.</param>
        Task SaveNotifyAsync(string notifyType, string notifyTitle, string notifyText, string notifyLvl, bool isUserNotify, string userId, string notifySysName);
    }
}