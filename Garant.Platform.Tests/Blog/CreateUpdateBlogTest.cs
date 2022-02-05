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
            mock.Setup(a => a.CreateBlogAsync("Всё про франчайзинг!","imagesFolderTest/image.png",false,1,2));

            //Act
            var component = new BlogRepository(PostgreDbContext, CommonService);
            var result = await component.CreateBlogAsync("Всё про франчайзинг!", "imagesFolderTest/image.png", false, 1, 2);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateBlogReturnsBlogOutputTest()
        {
            //Arrange

            var mock = new Mock<IBlogRepository>();
            mock.Setup(a => a.UpdateBlogAsync(1,"Как я закончил инвестировать!", "no-image.png", false, 3, 1));

            //Act
            var component = new BlogRepository(PostgreDbContext, CommonService);
            var result = await component.UpdateBlogAsync(1, "Как я закончил инвестировать!", "no-image.png", false, 3, 1);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
