using Autofac;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Abstractions.Control;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.MainPage;
using Garant.Platform.Abstractions.Pagination;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Abstractions.Search;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Core.Data;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Services.Service.Ad;
using Garant.Platform.Services.Control;
using Garant.Platform.Services.Document;
using Garant.Platform.Services.Request;
using Garant.Platform.Services.Service.Blog;
using Garant.Platform.Services.Service.Franchise;
using Garant.Platform.Services.Service.MainPage;
using Garant.Platform.Services.Service.Pagination;
using Garant.Platform.Services.Service.Search;
using Garant.Platform.Services.Service.User;
using Microsoft.AspNetCore.Identity;

namespace Garant.Platform.Services.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class ServicesModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис пользователя.
            builder.RegisterType<UserService>().Named<IUserService>("UserService");
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserRepository>().Named<IUserRepository>("UserRepository");
            builder.RegisterType<UserRepository>().As<IUserRepository>();
              
            // Общий сервис.
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
            builder.RegisterType<AdService>().As<IAdService>();

            // Сервис блогов.
            builder.RegisterType<BlogService>().Named<IBlogService>("BlogService");
            builder.RegisterType<BlogService>().As<IBlogService>();
            builder.RegisterType<BlogRepository>().Named<IBlogRepository>("BlogRepository");
            builder.RegisterType<BlogRepository>().As<IBlogRepository>();

            // Сервис пагинации.
            builder.RegisterType<PaginationService>().Named<IPaginationService>("PaginationService");
            builder.RegisterType<PaginationService>().As<IPaginationService>();

            // Сервис поиска.
            builder.RegisterType<SearchService>().Named<ISearchService>("PaginationService");
            builder.RegisterType<SearchService>().As<ISearchService>();

            // Сервис заявок.
            builder.RegisterType<RequestService>().Named<IRequestService>("RequestService");
            builder.RegisterType<RequestService>().As<IRequestService>();

            // Сервис документов.
            builder.RegisterType<DocumentService>().Named<IDocumentService>("DocumentService");
            builder.RegisterType<DocumentService>().As<IDocumentService>();
            builder.RegisterType<DocumentRepository>().Named<IDocumentRepository>("DocumentRepository");
            builder.RegisterType<DocumentRepository>().As<IDocumentRepository>();

            // Сервис контролов.
            builder.RegisterType<ControlService>().Named<IControlService>("ControlService");
            builder.RegisterType<ControlService>().As<IControlService>();
            builder.RegisterType<ControlRepository>().Named<IControlRepository>("ControlRepository");
            builder.RegisterType<ControlRepository>().As<IControlRepository>();
          
        }
    }
}
