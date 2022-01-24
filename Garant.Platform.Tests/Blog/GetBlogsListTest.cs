using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Services.Service.Blog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Blog
{
    [TestClass]
    public class GetBlogsListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetBlogsListAsyncMainPageTest()
        {
            var mock = new Mock<IBlogService>();
            mock.Setup(a => a.GetBlogsListMainPageAsync());
            var component = new BlogService(PostgreDbContext, BlogRepository);
            var result = await component.GetBlogsListMainPageAsync();

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task GetBlogsListAsyncTest()
        {
            var mock = new Mock<IBlogService>();
            mock.Setup(a => a.GetBlogsListAsync());
            var component = new BlogService(PostgreDbContext, BlogRepository);
            var result = await component.GetBlogsListAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
