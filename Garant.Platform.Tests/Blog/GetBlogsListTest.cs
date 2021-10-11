using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.Blog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Blog
{
    [TestClass]
    public class GetBlogsListTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetBlogsListAsyncTest()
        {
            var mock = new Mock<IBlogService>();
            mock.Setup(a => a.GetBlogsListMainPageAsync());
            var component = new BlogService(PostgreDbContext);
            var result = await component.GetBlogsListMainPageAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
