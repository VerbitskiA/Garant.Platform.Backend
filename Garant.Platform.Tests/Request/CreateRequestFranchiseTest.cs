using System.Threading.Tasks;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Services.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Request
{
    [TestClass]
    public class CreateRequestFranchiseTest : BaseServiceTest
    {
        [TestMethod]
        public async Task CreateRequestFranchiseAsyncTest()
        {
            var mock = new Mock<IRequestService>();
            mock.Setup(a => a.CreateRequestFranchiseAsync("Антон", "79845673245", "Москва", "ivan@mail.ru", 1000005));
            var component = new RequestService(FranchiseRepository, BusinessRepository, PostgreDbContext, RequestRepository);
            var result = await component.CreateRequestFranchiseAsync("Антон", "79845673245", "Москва", "ivan@mail.ru", 1000005);

            Assert.IsTrue(result != null);
        }
    }
}
