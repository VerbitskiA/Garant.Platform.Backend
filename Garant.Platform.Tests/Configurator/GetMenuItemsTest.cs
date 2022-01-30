using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Configurator
{
    [TestClass]
    public class GetMenuItemsTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetMenuItemsAsyncTest()
        {
            var result = await ConfiguratorService.GetMenuItemsAsync();

            Assert.IsTrue(result.Any());
        }
    }
}