using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.MainPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Category
{
    [TestClass]
    public class GetCategoriesListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetCategoriesListAsyncTest()
        {
            var mock = new Mock<IMainPageService>();
            mock.Setup(a => a.GetCategoriesListAsync());
            var component = new MainPageService(PostgreDbContext);
            var result = await component.GetCategoriesListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
