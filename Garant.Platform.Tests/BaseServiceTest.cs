using Garant.Platform.Base.Service;
using Garant.Platform.Core.Data;
using Garant.Platform.FTP.Service;
using Garant.Platform.Mailings.Service;
using Garant.Platform.Messaging.Service.Chat;
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
            FtpService = new FtpService(AppConfiguration, PostgreDbContext);

            // Настройка экземпляров сервисов для тестов.
            CommonService = new CommonService(PostgreDbContext, null);
            UserRepository = new UserRepository(PostgreDbContext, CommonService);
            FranchiseRepository = new FranchiseRepository(PostgreDbContext, UserRepository);
            BusinessRepository = new BusinessRepository(PostgreDbContext, UserRepository);
            PaginationRepository = new PaginationRepository(PostgreDbContext);

            MailingService = new MailingService(PostgreDbContext, AppConfiguration);
            UserService = new UserService(null, null, PostgreDbContext, MailingService, UserRepository, FtpService, CommonService);
            FranchiseService = new FranchiseService(PostgreDbContext, null, FranchiseRepository);
            BusinessRepository = new BusinessRepository(PostgreDbContext, UserRepository);
            ChatRepository = new ChatRepository(PostgreDbContext);
        }
    }
}
