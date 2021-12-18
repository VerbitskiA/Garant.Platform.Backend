using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Services.Document;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Document
{
    [TestClass]
    public class GetAttachmentDocumentNameVendorDealTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetAttachmentDocumentNameVendorDealAsyncTest()
        {
            var mock = new Mock<IDocumentRepository>();
            mock.Setup(a => a.GetAttachmentNameDocumentDealAsync("sierra_93@mail.ru"));
            var component = new DocumentRepository(PostgreDbContext, UserRepository);
            var result = await component.GetAttachmentNameDocumentDealAsync("sierra_93@mail.ru");

            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrEmpty(result.DocumentName));
        }
    }
}
