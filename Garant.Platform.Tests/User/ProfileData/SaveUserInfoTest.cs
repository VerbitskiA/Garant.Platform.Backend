using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.User.ProfileData
{
    [TestClass]
    public class SaveUserInfoTest : BaseServiceTest
    {
        [TestMethod]
        public async Task SaveUserInfoAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.SaveUserInfoAsync("Иван", "Сергеевич", "Самара", "sierra_93@mail.ru", "111", "buy,sell", 111222333, 444555666, "СберБанк", "sierra_93@mail.ru"));
            var component = new UserService(null, null, PostgreDbContext, MailingService, UserRepository, FtpService, CommonService);
            var result = await component.SaveUserInfoAsync("Иван", "Сергеевич", "Самара", "sierra_93@mail.ru", "111", "buy,sell", 111222333, 444555666, "СберБанк", "sierra_93@mail.ru");

            Assert.IsNotNull(result);
        }
    }
}
