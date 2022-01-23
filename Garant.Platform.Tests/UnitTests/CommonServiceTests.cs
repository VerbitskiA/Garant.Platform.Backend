using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garant.Platform.Tests.UnitTests
{
    [TestClass]
    public class CommonServiceTests : BaseServiceTest
    {
        [TestMethod]
        [DataRow(0, "дней")]
        [DataRow(1, "день")]
        [DataRow(2, "дня")]
        [DataRow(4, "дня")]
        [DataRow(5, "дней")]
        [DataRow(11, "дней")]
        [DataRow(21, "день")]
        [DataRow(33, "дня")]
        [DataRow(110, "дней")]
        [DataRow(111, "дней")]        
        [DataRow(115, "дней")]
        [DataRow(119, "дней")]
        [DataRow(1116, "дней")]
        [DataRow(1221, "день")]        
        public async Task GetCorrectDeclinationForDaysTest(int count, string expected)
        {
            string result = await CommonService.GetCorrectDeclinationForDays(count);

            Assert.AreEqual(expected, result);
        }
    }
}
