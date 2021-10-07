using Autofac;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Service.Service.Common;
using Garant.Platform.Service.Service.MainPage;
using Garant.Platform.Service.Service.User;
using Microsoft.AspNetCore.Identity;

namespace Garant.Platform.Service.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class CommonServicesModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис пользователя.
            builder.RegisterType<UserService>().Named<IUserService>("UserService");
            builder.RegisterType<UserService>().As<IUserService>();

            // Общий сервис.
            builder.RegisterType<CommonService>().Named<ICommonService>("CommonService");
            builder.RegisterType<CommonService>().As<ICommonService>();

            builder.RegisterType<SignInManager<UserEntity>>().InstancePerLifetimeScope();
            builder.RegisterType<UserManager<UserEntity>>().InstancePerLifetimeScope();

            // Сервис главной страницы.
            builder.RegisterType<MainPageService>().Named<IMainPageService>("MainPageService");
            builder.RegisterType<MainPageService>().As<IMainPageService>();
        }
    }
}
