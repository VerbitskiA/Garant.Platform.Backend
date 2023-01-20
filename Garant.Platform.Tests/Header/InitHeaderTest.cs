﻿using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Services.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Header
{
    [TestClass]
    public class InitHeaderTest : BaseServiceTest
    {
        [TestMethod]
        public async Task InitMainHeaderFieldsAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.InitHeaderAsync("Main"));
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            var result = await component.InitHeaderAsync("Main");

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task InitDopHeaderFieldsAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.InitHeaderAsync("Dop"));
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            var result = await component.InitHeaderAsync("Dop");

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task InitMainGarantHeaderFieldsAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.InitHeaderAsync("MainGarant"));
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            var result = await component.InitHeaderAsync("MainGarant");

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task InitDopGarantHeaderFieldsAsyncTest()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(a => a.InitHeaderAsync("DopGarant"));
            var component = new UserService(null, null, MailingService, UserRepository, FtpService, CommonService, null);
            var result = await component.InitHeaderAsync("DopGarant");

            Assert.IsTrue(result.Any());
        }
        
    }
}
