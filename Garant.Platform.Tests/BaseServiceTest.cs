using Garant.Platform.Base.Service;
using Garant.Platform.Configurator.Services;
using Garant.Platform.Core.Data;
using Garant.Platform.FTP.Service;
using Garant.Platform.Mailings.Service;
using Garant.Platform.Messaging.Service.Chat;
using Garant.Platform.Messaging.Service.Notifications;
using Garant.Platform.Services.Control;
using Garant.Platform.Services.Document;
using Garant.Platform.Services.Request;
using Garant.Platform.Services.Service.Blog;
using Garant.Platform.Services.Service.Business;
using Garant.Platform.Services.Service.Franchise;
using Garant.Platform.Services.Service.Pagination;
using Garant.Platform.Services.Service.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Garant.Platform.Tests
{
    /// <summary>
    /// Базовый класс тестов с настройками конфигурации.
    /// </summary>
    public class BaseServiceTest
    {
        protected string PostgreConfigString { get; set; }
        protected IConfiguration AppConfiguration { get; set; }

        protected PostgreDbContext PostgreDbContext;
        protected UserService UserService;
        protected CommonService CommonService;
        protected MailingService MailingService;
        protected FranchiseService FranchiseService;
        protected FranchiseRepository FranchiseRepository;
        protected UserRepository UserRepository;
        protected FtpService FtpService;
        protected BusinessRepository BusinessRepository;
        protected ChatRepository ChatRepository;
        protected PaginationRepository PaginationRepository;
        protected RequestService RequestService;
        protected DocumentService DocumentService;
        protected DocumentRepository DocumentRepository;
        protected ControlRepository ControlRepository;
        protected ControlService ControlService;
        protected BlogService BlogService;
        protected BlogRepository BlogRepository;
        protected ConfiguratorRepository ConfiguratorRepository;
        protected ConfiguratorService ConfiguratorService;
        protected BusinessService BusinessService;
        protected NotificationsService NotificationsService;
        protected NotificationsRepository NotificationsRepository;

        public BaseServiceTest()
        { 
            // Настройка тестовых строк подключения.
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            AppConfiguration = builder.Build();
            PostgreConfigString = AppConfiguration["ConnectionStrings:NpgTestSqlConnection"];

            // Настройка тестовых контекстов.
            var optionsBuilder = new DbContextOptionsBuilder<PostgreDbContext>();
            optionsBuilder.UseNpgsql(PostgreConfigString);
            PostgreDbContext = new PostgreDbContext(optionsBuilder.Options);
            FtpService = new FtpService(AppConfiguration);

            // Настройка экземпляров сервисов для тестов.
            CommonService = new CommonService(null);
            NotificationsService = new NotificationsService();
            NotificationsRepository = new NotificationsRepository();
            UserRepository = new UserRepository(CommonService);
            FranchiseRepository = new FranchiseRepository(UserRepository, CommonService);
            BusinessRepository = new BusinessRepository(UserRepository, CommonService);
            BusinessService = new BusinessService(BusinessRepository, FtpService, NotificationsService, UserRepository, NotificationsRepository);
            PaginationRepository = new PaginationRepository(CommonService);

            BlogRepository = new BlogRepository(UserRepository);
            BlogService = new BlogService(BlogRepository, FtpService);

            MailingService = new MailingService(AppConfiguration);
            UserService = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            FranchiseService = new FranchiseService(null, FranchiseRepository, NotificationsService, UserRepository, NotificationsRepository);            
            ChatRepository = new ChatRepository();
            RequestService = new RequestService(FranchiseRepository, BusinessRepository, NotificationsService);
            DocumentRepository = new DocumentRepository(PostgreDbContext, UserRepository);
            DocumentService = new DocumentService(PostgreDbContext, FtpService, DocumentRepository);
            ControlRepository = new ControlRepository();
            ControlService = new ControlService(ControlRepository, UserRepository);
            ConfiguratorRepository = new ConfiguratorRepository();
            ConfiguratorService = new ConfiguratorService(ConfiguratorRepository, FranchiseRepository, BusinessRepository, NotificationsService);
        }
    }
}
