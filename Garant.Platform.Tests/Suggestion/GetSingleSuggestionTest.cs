using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Suggestion
{
    [TestClass]
    public class GetSingleSuggestionTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetSingleSuggestionAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.GetSingleSuggestion(true, false));
            var component = new UserService(null, null, PostgreDbContext, MailingService, UserRepository, FtpService, CommonService);
            var result = await component.GetSingleSuggestion(true, false);

            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task GetAllSuggestionAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.GetAllSuggestionsAsync(false, true));
            var component = new UserService(null, null, PostgreDbContext, MailingService, UserRepository, FtpService, CommonService);
            var result = await component.GetAllSuggestionsAsync(false, true);

            Assert.IsTrue(result.Any());
        }
    }
}
