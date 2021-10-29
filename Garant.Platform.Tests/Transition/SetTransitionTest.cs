using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.Franchise;
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
            //var franchiseService = new FranchiseService(PostgreDbContext, null, UserService);
            mock.Setup(a => a.SetTransitionAsync("ivan@mail.ru", "Franchise", 1000002));
            var component = new UserService(null, null, PostgreDbContext, CommonService, MailingService);
            var result = await component.SetTransitionAsync("ivan@mail.ru", "Franchise", 1000002);

            Assert.IsTrue(result);
        }
    }
}
