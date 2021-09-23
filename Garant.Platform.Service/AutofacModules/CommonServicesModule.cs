using Autofac;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Service.Service.Common;
using Garant.Platform.Service.Service.User;

namespace Garant.Platform.Service.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class CommonServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Сервис пользователя.
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserService>().Named<IUserService>("UserService");

            // Общий сервис.
            builder.RegisterType<CommonService>().As<ICommonService>();
            builder.RegisterType<CommonService>().Named<ICommonService>("CommonService");
        }
    }
}
