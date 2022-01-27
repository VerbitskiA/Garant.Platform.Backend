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
    public class CreateBlogTest : BaseServiceTest
    {
        [TestMethod]
        public async Task CreateBlogReturnsBlogOutputTest()
        {
            //Arrange
           
            var mock = new Mock<IBlogRepository>();
            mock.Setup(a => a.CreateBlogAsync("Как я начал инвестировать!","imagesFolderTest/image.png",false,1,1));

            //Act
            var component = new BlogRepository(PostgreDbContext);
            var result = await component.CreateBlogAsync("Как я начал инвестировать!", "imagesFolderTest/image.png", false, 1, 1);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
