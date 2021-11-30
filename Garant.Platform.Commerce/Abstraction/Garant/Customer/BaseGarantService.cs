using System.Threading.Tasks;

namespace Garant.Platform.Commerce.Abstraction.Garant.Customer
{
    /// <summary>
    /// Базовый класс сервиса гаранта.
    /// </summary>
    /// <typeparam name="TService">Тип, с которым будет работать базовый сервис Гаранта.</typeparam>
    public abstract class BaseGarantService<TService>
    {
        public abstract Task PaymentActionAsync();
    }
}
