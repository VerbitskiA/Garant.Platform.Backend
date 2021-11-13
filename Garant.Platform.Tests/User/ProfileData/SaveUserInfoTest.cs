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
            mock.Setup(a => a.SaveUserInfoAsync("Иван", "Сергеевич", "Самара", "ivan@mail.ru", "111", "buy,sell"));
            var component = new UserService(null, null, PostgreDbContext, MailingService, UserRepository, FtpService);
            var result = await component.SaveUserInfoAsync("Иван", "Сергеевич", "Самара", "ivan@mail.ru", "111", "buy,sell");

            Assert.IsNotNull(result);
        }
    }
}
