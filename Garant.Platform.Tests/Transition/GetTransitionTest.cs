﻿using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction.User;
using Garant.Platform.Service.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Transition
{
    [TestClass]
    public class GetTransitionTest : BaseServiceTest
    {
        [TestMethod]
        public async Task GetTransitionAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.GetTransitionAsync("ivan@mail.ru"));
            var component = new UserService(null, null, PostgreDbContext, CommonService, MailingService, UserRepository);
            var result = await component.GetTransitionAsync("ivan@mail.ru");

            Assert.IsTrue(result != null);
        }
    }
}
