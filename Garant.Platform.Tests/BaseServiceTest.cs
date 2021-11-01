using Garant.Platform.Core.Data;
using Garant.Platform.Mailings.Service;
using Garant.Platform.Service.Repository.Franchise;
using Garant.Platform.Service.Repository.User;
using Garant.Platform.Service.Service.Common;
using Garant.Platform.Service.Service.Franchise;
using Garant.Platform.Service.Service.User;
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

            // Настройка экземпляров сервисов для тестов.
            CommonService = new CommonService(PostgreDbContext, null);
            UserRepository = new UserRepository(PostgreDbContext, CommonService);
            FranchiseRepository = new FranchiseRepository(PostgreDbContext, UserRepository);

            MailingService = new MailingService(PostgreDbContext, AppConfiguration);
            UserService = new UserService(null, null, PostgreDbContext, CommonService, MailingService, UserRepository);
            FranchiseService = new FranchiseService(PostgreDbContext, null, FranchiseRepository);
        }
    }
}
