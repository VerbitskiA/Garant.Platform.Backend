using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Header
{
    [TestClass]
    public class GetBreadcrumbsTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetBreadcrumbsAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.GetBreadcrumbsAsync("create-franchise"));
            var component = new UserService(null, null, PostgreDbContext, CommonService, MailingService);
            var result = await component.GetBreadcrumbsAsync("create-franchise");

            Assert.IsTrue(result.Any());
        }
    }
}
