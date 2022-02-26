using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.User.ProfileData
{
    [TestClass]
    public class GetProfileMenuTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetProfileMenuAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.GetProfileMenuListAsync());
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService);
            var result = await component.GetProfileMenuListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
