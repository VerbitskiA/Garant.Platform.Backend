using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.User;
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
            var component = new UserService(null, null, PostgreDbContext, CommonService);
            var result = await component.InitFooterAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
