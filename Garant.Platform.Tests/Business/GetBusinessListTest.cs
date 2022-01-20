using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Services.Service.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Business
{
    [TestClass]
    public class GetBusinessListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetBusinessListAsyncTest()
        {
            var mock = new Mock<IBusinessService>();
            mock.Setup(a => a.GetBusinessListAsync());
            var component = new BusinessService(PostgreDbContext, BusinessRepository, null);
            var result = await component.GetBusinessListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
