using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Service.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.User
{
    [TestClass]
    public class FindUserByEmailOrPhoneNumberTest : BaseServiceTest
    {
        [TestMethod]
        public async Task FindUserByEmailOrPhoneNumberAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.FindUserByEmailOrPhoneNumberAsync("ivan@mail.ru"));
            var component = new UserService(null, null, PostgreDbContext, CommonService, MailingService);
            var result = await component.FindUserByEmailOrPhoneNumberAsync("ivan@mail.ru");

            Assert.IsNotNull(result);
        }
    }
}
