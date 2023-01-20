using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.User.ProfileData
{
    [TestClass]
    public class GetProfileInfoTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetProfileInfo()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.GetProfileInfoAsync("sierra_93@mail.ru"));
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            var result = await component.GetProfileInfoAsync("sierra_93@mail.ru");

            Assert.IsNotNull(result);
        }
    }
}
