using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction.Franchise;
using Garant.Platform.Service.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class GetFranchisesCitiesListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetFranchisesCitiesListAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetFranchisesCitiesListAsync());
            var component = new FranchiseService(PostgreDbContext, null, UserService, FranchiseRepository);
            var result = await component.GetFranchisesCitiesListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
