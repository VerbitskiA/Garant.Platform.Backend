using Garant.Platform.Commerce.Abstraction.Garant.Vendor;
using Garant.Platform.Core.Data;

namespace Garant.Platform.Commerce.Service.Garant.Vendor
{
    /// <summary>
    /// Класс реализует методы Гаранта со стороны продавца.
    /// </summary>
    public sealed class VendorService : IVendorService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public VendorService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }
    }
}
