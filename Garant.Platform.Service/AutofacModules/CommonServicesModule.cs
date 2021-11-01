using Autofac;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Abstraction.Franchise;
using Garant.Platform.Core.Abstraction.User;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Core.Data;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Service.Pagination;
using Garant.Platform.Service.Repository.Franchise;
using Garant.Platform.Service.Repository.User;
using Garant.Platform.Service.Service.Ad;
using Garant.Platform.Service.Service.Blog;
using Garant.Platform.Service.Service.Common;
using Garant.Platform.Service.Service.Franchise;
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
            builder.RegisterType<UserRepository>().Named<IUserRepository>("UserRepository");
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            // Общий сервис.
            builder.RegisterType<CommonService>().Named<ICommonService>("CommonService");
            builder.RegisterType<CommonService>().As<ICommonService>();

            builder.RegisterType<SignInManager<UserEntity>>().InstancePerLifetimeScope();
            builder.RegisterType<UserManager<UserEntity>>().InstancePerLifetimeScope();

            // Сервис главной страницы.
            builder.RegisterType<MainPageService>().Named<IMainPageService>("MainPageService");
            builder.RegisterType<MainPageService>().As<IMainPageService>();

            // Сервис франшиз.
            builder.RegisterType<FranchiseService>().Named<IFranchiseService>("FranchiseService");
            builder.RegisterType<FranchiseService>().As<IFranchiseService>();
            builder.RegisterType<FranchiseRepository>().Named<IFranchiseRepository>("FranchiseRepository");
            builder.RegisterType<FranchiseRepository>().As<IFranchiseRepository>();

            builder.RegisterType<PostgreDbContext>().InstancePerLifetimeScope();

            // Сервис объявлений.
            builder.RegisterType<AdService>().Named<IAdService>("AdService");
            builder.RegisterType<AdService>().As<IFranchiseService>();

            // Сервис блогов.
            builder.RegisterType<BlogService>().Named<IBlogService>("BlogService");
            builder.RegisterType<BlogService>().As<IFranchiseService>();

            // Сервис пагинации.
            builder.RegisterType<PaginationService>().Named<IPaginationService>("PaginationService");
            builder.RegisterType<PaginationService>().As<IPaginationService>();
        }
    }
}
