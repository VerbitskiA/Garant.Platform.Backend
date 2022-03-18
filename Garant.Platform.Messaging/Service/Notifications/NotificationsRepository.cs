using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Models.Notification;

namespace Garant.Platform.Messaging.Service.Notifications
{
    /// <summary>
    /// Класс реализует методы репозитория уведомлений. 
    /// </summary>
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        
        public NotificationsRepository()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
        }
        
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
        public async Task SaveNotifyAsync(string notifyType, string notifyTitle, string notifyText, string notifyLvl, bool isUserNotify,
            string userId, string notifySysName)
        {
            try
            {
                await _postgreDbContext.Notifications.AddAsync(new NotificationEntity
                {
                    ActionNotifySysName = notifySysName,
                    DateCreate = DateTime.Now,
                    UserId = userId,
                    IsUserNotify = isUserNotify,
                    NotificationLevel = notifyLvl,
                    NotificationTitle = notifyTitle,
                    NotificationText = notifyText,
                    NotificationType = notifyType
                });
                await _postgreDbContext.SaveChangesAsync();
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}