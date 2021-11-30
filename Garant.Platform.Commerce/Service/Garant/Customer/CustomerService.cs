using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    /// <summary>
    /// Класс реализует методы Гаранта со стороны покупателя.
    /// </summary>
    public sealed class CustomerService : BaseGarantService<CustomerService>
    {
        public override Task PaymentActionAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
