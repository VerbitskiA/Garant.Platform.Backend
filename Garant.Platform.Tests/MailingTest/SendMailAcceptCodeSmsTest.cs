//using System.Threading.Tasks;
//using Garant.Platform.Core.Abstraction;
//using Garant.Platform.Service.Service.Common;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace Garant.Platform.Tests.MailingTest
//{
//    [TestClass]
//    public class SendMailAcceptCodeSmsTest : BaseServiceTest
//    {
//        [TestMethod]
//        public async Task SendMailAcceptCodeSmsAsyncTest()
//        {
//            var mock = new Mock<ICommonService>();
//            mock.Setup(a => a.GenerateAcceptCodeAsync("sms"));
//            var component = new CommonService();
//            await component.GenerateAcceptCodeAsync("sms");
//        }
//    }
//}
