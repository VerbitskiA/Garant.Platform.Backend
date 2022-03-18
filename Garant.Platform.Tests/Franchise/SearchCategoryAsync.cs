using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class SearchCategoryTestAsync : BaseServiceTest
    {
        [TestMethod]
        public async Task SearchCategoryAsyncTest()
        {
            var result = await FranchiseService.SearchCategoryAsync("Разливное пиво", "3d71b6a4-e682-4afd-b092-57d1578cb0b0", "Food");
            
            Assert.IsTrue(result.Any());
        }
        
        [TestMethod]
        public async Task SearchCategoryEpmtyParamAsyncTest()
        {
            var result = await FranchiseService.SearchCategoryAsync(string.Empty, "3d71b6a4-e682-4afd-b092-57d1578cb0b0", "Food");
            
            Assert.IsTrue(result.Count() > 1);
        }
    }
}