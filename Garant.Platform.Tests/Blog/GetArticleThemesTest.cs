using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Blog
{
    [TestClass]
    public class GetArticleThemesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetArticleThemesAsyncTest()
        {
            var result = await BlogService.GetArticleThemesAsync();
            
            Assert.IsTrue(result.Any());
        }
    }
}