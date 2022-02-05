using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Services.Service.Blog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Garant.Platform.Tests.Blog
{
    [TestClass]
    public class GetBlogThemesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetBlogThemesAsyncTest()
        {
            var mock = new Mock<IBlogService>();
            mock.Setup(a => a.GetBlogThemesAsync());
            var component = new BlogService(PostgreDbContext, BlogRepository,FtpService);
            var result = await component.GetBlogThemesAsync();

            Assert.IsTrue(result.Any());
        }
    }
}
