using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Messaging.Abstraction;
using Garant.Platform.Messaging.Service;

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
            builder.RegisterType<MessagingService>().Named<IMessagingService>("MessagingService");
            builder.RegisterType<MessagingService>().As<IMessagingService>();
        }
    }
}
