using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Services.Service.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Business
{
    [TestClass]
    public class GetPopularBusinessTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetPopularBusinessAsyncTest()
        {
            var mock = new Mock<IBusinessService>();
            mock.Setup(a => a.GetPopularBusinessAsync());
            var component = new BusinessService(PostgreDbContext, BusinessRepository, null);
            var result = await component.GetPopularBusinessAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
