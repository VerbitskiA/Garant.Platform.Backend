using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Footer
{
    [TestClass]
    public class InitFooterTest : BaseServiceTest
    {
        [TestMethod]
        public async Task InitFooterAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.InitFooterAsync());
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            var result = await component.InitFooterAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
