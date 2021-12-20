using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Document
{
    [TestClass]
    public class ApproveDocumentVendorTest : BaseServiceTest
    {
        [TestMethod]
        public async Task ApproveDocumentVendorAsyncTest()
        {
            var result = await DocumentRepository.ApproveDocumentVendorAsync(1000005, "sierra_93@mail.ru");

            Assert.IsTrue(result);
        }
    }
}
