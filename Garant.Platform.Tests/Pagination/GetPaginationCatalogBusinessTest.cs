using System.Threading.Tasks;
using Garant.Platform.Abstractions.Pagination;
using Garant.Platform.Services.Service.Pagination;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Pagination
{
    [TestClass]
    public class GetPaginationCatalogBusinessTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetInitPaginationCatalogBusinessAsyncTest()
        {
            var mock = new Mock<IPaginationService>();
            mock.Setup(a => a.GetInitPaginationCatalogBusinessAsync(1));
            var component = new PaginationService(PostgreDbContext, PaginationRepository);
            var result = await component.GetInitPaginationCatalogBusinessAsync(1);

            Assert.IsTrue(result.Results != null);
        }

        [TestMethod]
        public async Task GetPaginationCatalogBusinessAsyncTest()
        {
            var mock = new Mock<IPaginationService>();
            mock.Setup(a => a.GetPaginationCatalogBusinessAsync(1, 10));
            var component = new PaginationService(PostgreDbContext, PaginationRepository);
            var result = await component.GetPaginationCatalogBusinessAsync(1, 10);

            Assert.IsTrue(result.Results != null);
        }
    }
}
