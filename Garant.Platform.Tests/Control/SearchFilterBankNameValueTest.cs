using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Control
{
    [TestClass]
    public class SearchFilterBankNameValueTest : BaseServiceTest
    {
        [TestMethod]
        public async Task SearchFilterBankNameValueAsyncTest()
        {
            var result = await ControlService.SearchFilterBankNameValueAsync("сбер");

            Assert.IsTrue(result.Any());
        }
    }
}
