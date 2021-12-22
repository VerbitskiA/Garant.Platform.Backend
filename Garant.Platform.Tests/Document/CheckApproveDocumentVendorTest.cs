using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Services.Document;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Garant.Platform.Tests.Document
{
    /// <summary>
    /// Класс теста проверит, подтвержден ли договор продавца покупателем.
    /// </summary>
    [TestClass]
    public class CheckApproveDocumentVendorTest : BaseServiceTest
    {
        /// <summary>
        /// Если подтвердил.
        /// </summary>
        [TestMethod]
        public async Task CheckIsTrueApproveDocumentVendorAsyncTest()
        {
            var mock = new Mock<IDocumentRepository>();
            mock.Setup(a => a.CheckApproveDocumentVendorAsync(1000005));
            var component = new DocumentRepository(PostgreDbContext, UserRepository);
            var result = await component.CheckApproveDocumentVendorAsync(1000005);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Если не подтвердил.
        /// </summary>
        [TestMethod]
        public async Task CheckIsFalseApproveDocumentVendorAsyncTest()
        {
            var mock = new Mock<IDocumentRepository>();
            mock.Setup(a => a.CheckApproveDocumentVendorAsync(1000005));
            var component = new DocumentRepository(PostgreDbContext, UserRepository);
            var result = await component.CheckApproveDocumentVendorAsync(1000005);

            Assert.IsFalse(result);
        }
    }
}
