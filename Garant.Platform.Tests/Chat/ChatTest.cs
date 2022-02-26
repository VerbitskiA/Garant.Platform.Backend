using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Messaging.Abstraction.Chat;
using Garant.Platform.Messaging.Service.Chat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Chat
{
    [TestClass]
    public class ChatTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetCreateDialogAsync()
        {
            var mock = new Mock<IChatService>();
            mock.Setup(a => a.GetDialogAsync(null, "sierra_93@mail.ru", "53244c01-ac81-49d2-a958-9a08e06b45cc", "Franchise"));
            var component = new ChatService(ChatRepository, UserRepository, FranchiseRepository, BusinessRepository);
            var result = await component.GetDialogAsync(null, "sierra_93@mail.ru", "53244c01-ac81-49d2-a958-9a08e06b45cc", "Franchise");

            Assert.IsTrue(result.Messages.Any());
        }

        [TestMethod]
        public async Task SendMessageAsync()
        {
            var mock = new Mock<IChatService>();
            mock.Setup(a => a.SendMessageAsync("test", "sierra_93@mail.ru", 1));
            var component = new ChatService(ChatRepository, UserRepository, FranchiseRepository, BusinessRepository);
            var result = await component.SendMessageAsync("test", "sierra_93@mail.ru", 1);

            Assert.IsTrue(result.Messages.Any());
        }

        [TestMethod]
        public async Task GetDialogsAsync()
        {
            var mock = new Mock<IChatService>();
            mock.Setup(a => a.GetDialogsAsync("sierra_93@mail.ru"));
            var component = new ChatService(ChatRepository, UserRepository, FranchiseRepository, BusinessRepository);
            var result = await component.GetDialogsAsync("sierra_93@mail.ru");

            Assert.IsTrue(result.Any());
        }
    }
}
