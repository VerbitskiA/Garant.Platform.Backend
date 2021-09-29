using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Mailings.Service.Sms;

namespace Garant.Platform.Mailings.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    public sealed class MailingModule
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис смс-рассылок.
            builder.RegisterType<MailingSmsService>().Named<IMailingSmsService>("MailingSmsService");
            builder.RegisterType<MailingSmsService>().As<IMailingSmsService>();
        }
    }
}
