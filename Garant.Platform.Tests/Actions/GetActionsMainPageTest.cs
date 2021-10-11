using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.MainPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Actions
{
    [TestClass]
    public class GetActionsMainPageTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetActionsMainPageAsyncTest()
        {
            var mock = new Mock<IMainPageService>();
            mock.Setup(a => a.GetActionsMainPageAsync());
            var component = new MainPageService(PostgreDbContext);
            var result = await component.GetActionsMainPageAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
