using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.Franchise;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class GetMainPopularFranchisesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetMainPopularFranchisesAsyncTest()
        {
            var mock = new Mock<IFranchiseService>();
            mock.Setup(a => a.GetMainPopularFranchises());
            var component = new FranchiseService(PostgreDbContext);
            var result = await component.GetMainPopularFranchises();

            Assert.IsTrue(result.Any());
        }

        //[TestMethod]
        //public async Task GetPopularFranchisesAsyncTest()
        //{
        //    var mock = new Mock<IFranchiseService>();
        //    mock.Setup(a => a.GetMainPopularFranchises());
        //    var component = new FranchiseService(PostgreDbContext);
        //    var result = await component.GetMainPopularFranchises();

        //    Assert.IsTrue(result.Any());
        //}
    }
}
