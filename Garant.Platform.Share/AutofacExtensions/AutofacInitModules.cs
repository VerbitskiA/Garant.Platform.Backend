using Autofac;
using Garant.Platform.Mailings.AutofacModules;
using Garant.Platform.Service.AutofacModules;

namespace Garant.Platform.Share.AutofacExtensions
{
    public static class AutofacInitModules
    {
        public static void InitModules(ContainerBuilder builder)
        {
            builder.RegisterModule(new MailingModule());
            builder.RegisterModule(new CommonServicesModule());
        }
    }
}
