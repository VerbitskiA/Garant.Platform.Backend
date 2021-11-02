using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction.User;
using Garant.Platform.Service.Service.User;
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
            mock.Setup(a => a.SetTransitionAsync("ivan@mail.ru", "Franchise", 1000002));
            var component = new UserService(null, null, PostgreDbContext, MailingService, UserRepository);
            var result = await component.SetTransitionAsync("ivan@mail.ru", "Franchise", 1000002);

            Assert.IsTrue(result);
        }
    }
}
