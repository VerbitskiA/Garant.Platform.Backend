using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.MainPage;
using Garant.Platform.Services.Service.MainPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.LastBuy
{
    [TestClass]
    public class GetSliderLastBuyTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetSliderLastBuyAsyncTest()
        {
            var mock = new Mock<IMainPageService>();
            mock.Setup(a => a.GetSliderLastBuyAsync());
            var component = new MainPageService(PostgreDbContext);
            var result = await component.GetSliderLastBuyAsync();

            Assert.IsTrue(result.Count() < 6);
        }
    }
}
