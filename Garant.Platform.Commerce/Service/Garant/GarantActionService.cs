using System;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Models.Garant.Output;

namespace Garant.Platform.Commerce.Service.Garant
{
    /// <summary>
    /// Класс реализует методы сервиса Гаранта, где определяются различные действия.
    /// </summary>
    public sealed class GarantActionService : IGarantActionService
    {
        /// <summary>
        /// Метод определит действия при попадании в Гарант.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса, с которым зашли в Гарант.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="orderType">Тип заказа франшиза или бизнес..</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные успешного платежа.</returns>
        public Task<PaymentActionOutput> GetTypeGarantAsync(long originalId, string orderType, double amount, string account)
        {
            throw new NotImplementedException();
        }
    }
}
