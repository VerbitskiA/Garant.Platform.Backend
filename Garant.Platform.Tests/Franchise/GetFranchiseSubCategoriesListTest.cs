using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Services.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class GetFranchiseSubCategoriesListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetFranchiseSubCategoriesListAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetSubCategoryListAsync("0599ef42-ddf6-4c5a-b406-127d5e867ef6", "Auto"));
            var component = new FranchiseService(null, FranchiseRepository);
            var result = await component.GetSubCategoryListAsync("0599ef42-ddf6-4c5a-b406-127d5e867ef6", "Auto");

            Assert.IsTrue(result.Any());
        }
    }
}
