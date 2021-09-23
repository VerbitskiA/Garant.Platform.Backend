using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Mailings.Service.Sms;

namespace Garant.Platform.Mailings.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class MailingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //// Сервис смс-рассылок.
            builder.RegisterType<MailingSmsService>().Named<IMailingSmsService>("MailingSmsService");
            builder.RegisterType<MailingSmsService>().As<IMailingSmsService>();
        }
    }
}
