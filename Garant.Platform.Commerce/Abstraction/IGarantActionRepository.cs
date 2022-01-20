using System;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Models.Entities.Commerce;

namespace Garant.Platform.Commerce.Abstraction
{
    /// <summary>
    /// Абстракция репозитория событий Гаранта для работы с БД.
    /// </summary>
    public interface IGarantActionRepository
    {
        /// <summary>
        /// Метод получит дату создания сделки.
        /// </summary>
        /// <param name="userId">Id владельца предмета сделки.</param>
        /// <returns>Дату создания сделки.</returns>
        Task<DateTime> GetOwnerIdItemDealAsync(string userId);

        /// <summary>
        /// Метод найдет Id пользователя, который создал заказ.
        /// </summary>
        /// <param name="orderId">Id заказа.</param>
        /// <returns>Id пользователя.</returns>
        Task<string> GetUserIdCreatedOrderAsync(long orderId);

        /// <summary>
        /// Метод найдет телефон и почту пользователя создавшего заказ.
        /// <param name="userId">Id пользователя.</param>
        /// </summary>
        /// <returns>Данные пользователя.</returns>
        Task<FindUserPhoneEmailCreatedOrderOutput> FindUserEmailPhoneCreatedOrderAsync(string userId);

        /// <summary>
        /// Метод найдет все данные заказа по его Id.
        /// </summary>
        /// <param name="orderId">Id заказа в сервисе Гарант.</param>
        /// <returns>Данне заказа.</returns>
        Task<OrderEntity> GetOrderByIdAsync(long orderId);

        /// <summary>
        /// Метод запишет в БД новый платеж.
        /// </summary>
        /// <param name="paymentId">Id платежа в сервисе Гарант.</param>
        /// <param name="accountNumberPayer">Номер расчетного счета продавца.</param>
        /// <param name="recipientName">Наименование получателя.</param>
        /// <param name="recipientInn">ИНН получателя.</param>
        /// <param name="recipientKpp">КПП получателя.</param>
        /// <param name="recipientBik">БИК получателя.</param>
        /// <param name="bankName">Название банка.</param>
        /// <param name="corrAccountNumber">Корреспондентский счёт банка получателя.</param>
        /// <param name="recipientAccountNumber">Номер расчетного счета получателя.</param>
        /// <param name="purpose">Назначение платежа.</param>
        /// <param name="amount">Сумма платежа в руб.</param>
        /// <param name="collectionAmount">Удержанная сумма в руб.</param>
        Task SetPaymentAsync(long? paymentId, string accountNumberPayer, string recipientName, string recipientInn, string recipientKpp, string recipientBik, string bankName, string corrAccountNumber, string recipientAccountNumber, string purpose, double amount, double? collectionAmount);

        /// <summary>
        /// Метод найдет системный Id заказа.
        /// </summary>
        /// <param name="systemOrderId">Системный Id заказа в сервисе Гарант.</param>
        /// <returns>Системный Id заказа в системе банка.</returns>
        Task<OrderEntity> GetOrderBySystemIdAsync(long systemOrderId);

        /// <summary>
        /// Метод получит Id последнего платежа.
        /// </summary>
        /// <returns>Id платежа.</returns>
        Task<long> GetLastPaymentIdAsync();
    }
}
