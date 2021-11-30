using Garant.Platform.Core.Data;

namespace Garant.Platform.Commerce.Abstraction.Garant.Customer
{
    /// <summary>
    /// Базовый репозиторий Гаранта для работы с БД.
    /// </summary>
    /// <typeparam name="TService">Тип, с которым будет работать базовый репозиторий Гаранта.</typeparam>
    public abstract class BaseGarantRepository<TService>
    {
        private readonly PostgreDbContext _postgreDbContext;

        public BaseGarantRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }
    }
}
