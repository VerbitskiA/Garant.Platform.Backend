using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Service.Notifications;

namespace Garant.Platform.Messaging.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public class MessagingModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис сообщений.
            // builder.RegisterType<RabbitMqService>().Named<IRabbitMqService>("RabbitMqService");
            // builder.RegisterType<RabbitMqService>().As<IRabbitMqService>();
            
            builder.RegisterType<NotificationsService>().Named<INotificationsService>("NotificationsService");
            builder.RegisterType<NotificationsService>().As<INotificationsService>();
        }
    }
}
