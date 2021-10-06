using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Mailings.Service;

namespace Garant.Platform.Mailings.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class MailingModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис смс-рассылок.
            builder.RegisterType<MailingService>().Named<IMailingService>("MailingSmsService");
            builder.RegisterType<MailingService>().As<IMailingService>();
        }
    }
}
