using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Services.Service.Blog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Garant.Platform.Tests.Blog
{
    [TestClass]
    public class CreateUpdateBlogTest : BaseServiceTest
    {
        [TestMethod]
        public async Task CreateBlogReturnsBlogOutputTest()
        {
            //Arrange
           
            var mock = new Mock<IBlogRepository>();
            mock.Setup(a => a.CreateBlogAsync("Всё про франчайзинг!", "imagesFolderTest/image.png", "77a580db-387a-480d-b759-6496caad0172"));

            //Act
            var component = new BlogRepository(PostgreDbContext, UserRepository);
            var result = await component.CreateBlogAsync("Всё про франчайзинг!", "imagesFolderTest/image.png", "77a580db-387a-480d-b759-6496caad0172");

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateBlogReturnsBlogOutputTest()
        {
            //Arrange

            var mock = new Mock<IBlogRepository>();
            mock.Setup(a => a.UpdateBlogAsync(1,"Как я закончил инвестировать!", "no-image.png", false, 3, "77a580db-387a-480d-b759-6496caad0172"));

            //Act
            var component = new BlogRepository(PostgreDbContext, UserRepository);
            var result = await component.UpdateBlogAsync(1, "Как я закончил инвестировать!", "no-image.png", false, 3, "77a580db-387a-480d-b759-6496caad0172");

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
