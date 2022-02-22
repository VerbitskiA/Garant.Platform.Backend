using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Configurator
{
    [TestClass]
    public class AcceptCardTest : BaseServiceTest
    {
        [TestMethod]
        public async Task AcceptFranchiseCardAsyncTest()
        {
            var result = await ConfiguratorService.AcceptCardAsync(4, "Franchise");
            
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public async Task AcceptBusinessCardAsyncTest()
        {
            var result = await ConfiguratorService.AcceptCardAsync(4, "Business");
            
            Assert.IsTrue(result);
        }
    }
}