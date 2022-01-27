using Autofac;
using Garant.Platform.Core.Attributes;
// using Garant.Platform.Messaging.Abstraction.RabbitMq;
// using Garant.Platform.Messaging.Service.RabbitMq;

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
        }
    }
}
