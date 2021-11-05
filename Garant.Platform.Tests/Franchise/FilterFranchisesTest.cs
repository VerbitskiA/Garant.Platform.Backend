using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.MainPage;
using Garant.Platform.Services.Service.MainPage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Franchise
{
    [TestClass]
    public class FilterFranchisesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task FilterFranchisesAsyncTest()
        {
            var mock = new Mock<IMainPageService>();
            mock.Setup(a => a.FilterFranchisesAsync("Франшиза", "Медицинский центр", "Москва", 12400000, 12500000));
            var component = new MainPageService(PostgreDbContext);
            var result = await component.FilterFranchisesAsync("Франшиза", "Медицинский центр", "Москва", 12400000, 12500000);

            Assert.IsTrue(result.Any());
        }
    }
}
