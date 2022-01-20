using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Transition
{
    [TestClass]
    public class SetTransitionTest : BaseServiceTest
    {
        [TestMethod]
        public async Task SetTransitioAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.SetTransitionAsync("ivan@mail.ru", "Franchise", 1000002, null, null));
            var component = new UserService(null, null, PostgreDbContext, MailingService, UserRepository, FtpService, CommonService);
            var result = await component.SetTransitionAsync("ivan@mail.ru", "Franchise", 1000002, null, null);

            Assert.IsTrue(result);
        }
    }
}
