using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Configurator
{
    [TestClass]
    public class RejectCardTest : BaseServiceTest
    {
        [TestMethod]
        public async Task RejectCardFranchiseAsyncTest()
        {
            var result = await ConfiguratorService.RejectCardAsync(1000005, "Franchise", "Тестовая причина отклонения");
            
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public async Task RejectCardBusinessAsyncTest()
        {
            var result = await ConfiguratorService.RejectCardAsync(1000018, "Business", "Тестовая причина отклонения");
            
            Assert.IsTrue(result);
        }
    }
}