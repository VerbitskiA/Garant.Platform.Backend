using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Garant.Platform.Tests.Document
{
    [TestClass]
    public class GetApproveVendorActsTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetApproveVendorActsAsyncTest()
        {
            var result = await DocumentRepository.GetApproveVendorActsAsync(1000005);

            Assert.IsTrue(result.Any());
        }
    }
}
