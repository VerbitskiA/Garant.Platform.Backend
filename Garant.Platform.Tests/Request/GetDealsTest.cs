using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.Request
{
    [TestClass]
    public class GetDealsTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetDealsAsyncTest()
        {
            var result = await RequestService.GetDealsAsync("lelya.ivanov.2023@list.ru");

            Assert.IsTrue(result.Any());
        }
    }
}