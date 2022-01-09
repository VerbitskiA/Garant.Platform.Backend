using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Control
{
    [TestClass]
    public class GetFilterBankNameValuesTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetFilterBankNameValuesAsyncTest()
        {
            var result = await ControlService.GetFilterBankNameValuesAsync("sierra_93@mail.ru");

            Assert.IsTrue(result.Any());
        }
    }
}
