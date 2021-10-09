using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.Ad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Ad
{
    [TestClass]
    public class GetNewAdsTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetNewAdsAsyncTest()
        {
            var mock = new Mock<IAdService>();
            mock.Setup(a => a.GetNewAdsAsync());
            var component = new AdService(PostgreDbContext);
            var result = await component.GetNewAdsAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
