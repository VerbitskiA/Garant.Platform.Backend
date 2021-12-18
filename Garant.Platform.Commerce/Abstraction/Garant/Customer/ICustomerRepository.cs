using System;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Models.Commerce.Output;

namespace Garant.Platform.Commerce.Abstraction.Garant.Customer
{
    /// <summary>
    /// Абстракция репозитория Гаранта со стороны покупателя для работы с БД.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Метод создаст новый заказ.
        /// </summary>
        /// <param name="amount">Цена.</param>
        /// <param name="endDate">Дата окончания холдирования.</param>
        /// <param name="description">Объект с описанием платежа.</param>
        /// <param name="redirectUrl">Url редиректа после успешного платежа.</param>
        /// <returns>Данные платежа.</returns>
        Task<OrderOutput> CreateOrderAsync(long originalId, double amount, DateTime endDate, Description description, string redirectUrl, string orderType, string userId);
    }
}
