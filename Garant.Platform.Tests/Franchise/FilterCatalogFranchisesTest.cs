using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction.Franchise;
using Garant.Platform.Service.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class FilterCatalogFranchisesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task FilterFranchisesAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.FilterFranchisesAsync("Desc", "1000", "100000", false));
            var component = new FranchiseService(PostgreDbContext, null, UserService, FranchiseRepository);
            var result = await component.FilterFranchisesAsync("Desc", "1000", "100000", false);

            Assert.IsTrue(result.Any());
        }
    }
}
