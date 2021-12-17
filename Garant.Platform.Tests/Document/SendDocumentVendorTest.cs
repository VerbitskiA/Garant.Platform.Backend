using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Services.Document;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Document
{
    [TestClass]
    public class SendDocumentVendorTest : BaseServiceTest
    {
        [TestMethod]
        public async Task SendDocumentVendorAsyncTest()
        {
            var mock = new Mock<IDocumentService>();
            mock.Setup(a => a.SendDocumentVendorAsync(1, true, "DocumentVendor"));
            var component = new DocumentService(PostgreDbContext, FtpService, DocumentRepository);
            var result = await component.SendDocumentVendorAsync(1, true, "DocumentVendor");

            Assert.IsTrue(result);
        }
    }
}
