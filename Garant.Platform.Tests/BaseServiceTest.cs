using Garant.Platform.Core.Data;
using Garant.Platform.Mailings.Service.Sms;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Service.Service.Common;
using Garant.Platform.Service.Service.User;
using Microsoft.AspNetCore.Identity;
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
        protected MailingSmsService MailingSmsService;
        protected readonly SignInManager<UserEntity> SignInManager;
        protected readonly UserManager<UserEntity> UserManager;

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
            MailingSmsService = new MailingSmsService();
            CommonService = new CommonService(PostgreDbContext, MailingSmsService);
            //SignInManager = new SignInManager<UserEntity>();
            //UserManager = new UserManager<UserEntity>();
            UserService = new UserService(CommonService, null, null, PostgreDbContext);
        }
    }
}
