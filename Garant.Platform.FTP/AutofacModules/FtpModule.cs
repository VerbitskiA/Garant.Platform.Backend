using Autofac;
using Garant.Platform.Core.Attributes;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.FTP.Service;

namespace Garant.Platform.FTP.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class FtpModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис FTP.
            builder.RegisterType<FtpService>().Named<IFtpService>("FtpService");
            builder.RegisterType<FtpService>().As<IFtpService>();
        }
    }
}
