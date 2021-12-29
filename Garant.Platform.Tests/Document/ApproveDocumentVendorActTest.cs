using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Document
{
    [TestClass]
    public class ApproveDocumentVendorActTest : BaseServiceTest
    {
        [TestMethod]
        public async Task ApproveDocumentVendorActAsyncTest()
        {
            var result = await DocumentRepository.ApproveActVendorAsync(1000005, "DocumentVendorAct1");

            Assert.IsTrue(result);
        }
    }
}
