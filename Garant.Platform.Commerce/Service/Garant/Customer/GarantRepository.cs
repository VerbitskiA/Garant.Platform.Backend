using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Core.Data;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    public sealed class GarantRepository : BaseGarantRepository<GarantRepository>
    {
        public GarantRepository(PostgreDbContext postgreDbContext) : base(postgreDbContext)
        {
        }
    }
}
