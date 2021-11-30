using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    /// <summary>
    /// Класс реализует методы Гаранта со стороны продавца.
    /// </summary>
    public sealed class VendorService : BaseGarantService<VendorService>
    {
        public override Task PaymentActionAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
