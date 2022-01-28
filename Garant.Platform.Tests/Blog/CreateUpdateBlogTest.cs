using Garant.Platform.Abstractions.Blog;
using Garant.Platform.Core.Data;
using Garant.Platform.Services.Service.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var component = new BlogRepository(PostgreDbContext);
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
            var component = new BlogRepository(PostgreDbContext);
            var result = await component.UpdateBlogAsync(1, "Как я закончил инвестировать!", "no-image.png", false, 3, 1);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
