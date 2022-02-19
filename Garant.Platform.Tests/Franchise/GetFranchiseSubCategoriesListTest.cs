// using System.Linq;
// using System.Threading.Tasks;
// using Garant.Platform.Abstractions.Franchise;
// using Garant.Platform.Services.Service.Franchise;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Moq;
//
// namespace Garant.Platform.Tests.Franchise
// {
//     [TestClass]
//     public class GetFranchiseSubCategoriesListTest : BaseServiceTest
//     {
//         [TestMethod]
//         public async Task GetFranchiseSubCategoriesListAsyncTest()
//         {
//             var mock = new Mock<IFranchiseService>();
//             mock.Setup(a => a.GetSubCategoryListAsync());
//             var component = new FranchiseService(PostgreDbContext, null, FranchiseRepository);
//             var result = await component.GetSubCategoryListAsync();
//
//             Assert.IsTrue(result.Any());
//         }
//     }
// }
