using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Entities.Commerce;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Commerce.Service.Garant
{
    /// <summary>
    /// Класс реализует методы репозитория событий Гаранта.
    /// </summary>
    public class GarantActionRepository : IGarantActionRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public GarantActionRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод получит дату создания сделки.
        /// </summary>
        /// <param name="userId">Id владельца предмета сделки.</param>
        /// <returns>Дату создания сделки.</returns>
        public async Task<DateTime> GetOwnerIdItemDealAsync(string userId)
        {
            try
            {
                var result = await _postgreDbContext.Deals
                    .Where(d => d.UserId.Equals(userId))
                    .Select(d => d.DateCreate)
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет Id пользователя, который создал заказ.
        /// </summary>
        /// <param name="orderId">Id заказа.</param>
        /// <returns>Id пользователя.</returns>
        public async Task<string> GetUserIdCreatedOrderAsync(long orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    return null;
                }

                var userId = await _postgreDbContext.Orders
                    .Where(o => o.OrderId == orderId)
                    .Select(o => o.UserId)
                    .FirstOrDefaultAsync();

                return userId;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет телефон и почту пользователя создавшего заказ.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<FindUserPhoneEmailCreatedOrderOutput> FindUserEmailPhoneCreatedOrderAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                var result = await _postgreDbContext.Users
                    .Where(u => u.Id.Equals(userId))
                    .Select(u => new FindUserPhoneEmailCreatedOrderOutput
                    {
                        Email = u.Email,
                        Phone = u.PhoneNumber
                    })
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет все данные заказа по его Id.
        /// </summary>
        /// <param name="orderId">Id заказа в сервисе Гарант.</param>
        /// <returns>Данные заказа.</returns>
        public async Task<OrderEntity> GetOrderByIdAsync(long orderId)
        {
            try
            {
                if (orderId <= 0)
                {
                    return null;
                }

                // ИНайдет по Id предмета сделки.
                var result = await _postgreDbContext.Orders
                    .OrderByDescending(o => o.OrderId)
                    .Where(o => o.OriginalId == orderId
                                || o.TinkoffSystemOrderId == orderId
                                || o.OrderId == orderId)
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

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
        public async Task SetPaymentAsync(long? paymentId, string accountNumberPayer, string recipientName, string recipientInn, string recipientKpp, string recipientBik, string bankName, string corrAccountNumber, string recipientAccountNumber, string purpose, double amount, double? collectionAmount)
        {
            try
            {
                // Если не нужно создавать платеж.
                if (paymentId == null)
                {
                    return;
                }

                await _postgreDbContext.Payments.AddAsync(new PaymentEntity
                {
                    PaymentId = Convert.ToInt64(paymentId),
                    AccountNumberPayer = accountNumberPayer,
                    Amount = amount,
                    BankName = bankName,
                    CollectionAmount = collectionAmount,
                    CorrAccountNumber = corrAccountNumber,
                    Purpose = purpose,
                    RecipientAccountNumber = recipientAccountNumber,
                    RecipientBik = recipientBik,
                    RecipientInn = recipientInn,
                    RecipientName = recipientName,
                    RecipientKpp = recipientKpp
                });

                await _postgreDbContext.SaveChangesAsync();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет системный Id заказа.
        /// </summary>
        /// <param name="systemOrderId">Системный Id заказа в сервисе Гарант.</param>
        /// <returns>Системный Id заказа в системе банка.</returns>
        public async Task<OrderEntity> GetOrderBySystemIdAsync(long systemOrderId)
        {
            try
            {
                if (systemOrderId <= 0)
                {
                    return null;
                }

                var result = await _postgreDbContext.Orders.FirstOrDefaultAsync(o => o.TinkoffSystemOrderId == systemOrderId);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит Id последнего платежа.
        /// </summary>
        /// <returns>Id платежа.</returns>
        public async Task<long> GetLastPaymentIdAsync()
        {
            try
            {
                if (!await _postgreDbContext.Payments.AnyAsync())
                {
                    return 0;
                }

                var result = await _postgreDbContext.Payments
                    .OrderByDescending(p => p.PaymentId)
                    .Select(p => p.PaymentId)
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        public async  Task<long> GetDealIdAsync(string userId)
        {
            try
            {
                var result = await _postgreDbContext.Deals
                    .Where(d => d.UserId.Equals(userId))
                    .Select(d => d.DealId)
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}
