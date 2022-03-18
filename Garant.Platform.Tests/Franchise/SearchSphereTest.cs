using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class SearchSphereTest : BaseServiceTest
    {
        [TestMethod]
        public async Task SearchSphereAsyncTest()
        {
            var result = await FranchiseService.SearchSphereAsync("Питание");
            
            Assert.IsTrue(result.Any());
        }
        
        [TestMethod]
        public async Task SearchSphereEpmtyParamAsyncTest()
        {
            var result = await FranchiseService.SearchSphereAsync(string.Empty);
            
            Assert.IsTrue(result.Count() > 1);
        }
    }
}