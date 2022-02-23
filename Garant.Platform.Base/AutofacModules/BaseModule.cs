using Autofac;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Base.Service;
using Garant.Platform.Core.Attributes;

namespace Garant.Platform.Base.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class BaseModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            builder.RegisterType<CommonService>().Named<ICommonService>("CommonService");
            builder.RegisterType<CommonService>().As<ICommonService>();
            builder.RegisterType<CommonRepository>().Named<ICommonRepository>("CommonRepository");
            builder.RegisterType<CommonRepository>().As<ICommonRepository>();
        }
    }
}
