using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Garant.Platform.Tests.User.GetUsers
{
    [TestClass]
    public class GetUsersTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetUsersTestAsyncTest()
        {
            var res = await PostgreDbContext.Users.ToListAsync();
            Console.WriteLine();
        }
    }
}
