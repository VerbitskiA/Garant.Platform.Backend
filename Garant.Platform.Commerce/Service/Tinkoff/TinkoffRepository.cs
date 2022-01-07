using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Core.Exceptions;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Commerce.Service.Tinkoff
{
    /// <summary>
    /// Класс реализует методы репозитория платежной системы Тинькофф.
    /// </summary>
    public sealed class TinkoffRepository : ITinkoffRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IGarantActionRepository _garantActionRepository;

        public TinkoffRepository(PostgreDbContext postgreDbContext, IGarantActionRepository garantActionRepository)
        {
            _postgreDbContext = postgreDbContext;
            _garantActionRepository = garantActionRepository;
        }

        /// <summary>
        /// Метод получит ссылку для оплаты.
        /// </summary>
        /// <returns>Ссылка на оплату.</returns>
        public async Task<string> GetReturnForPaymentUrlAsync()
        {
            try
            {
                var result = await _postgreDbContext.ReturnUrls
                    .Where(l => l.TypeLink.Equals("Payment"))
                    .Select(l => l.Link)
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
        /// Метод запишет Id заказа в системе банка.
        /// </summary>
        /// <param name="orderId">Id заказа в сервисе Гарант.</param>
        /// <param name="systemOrderId">Id платежа в системе банка.</param>
        public async Task SetSystemOrderIdAsync(long orderId, long systemOrderId)
        {
            try
            {
                if (systemOrderId <= 0)
                {
                    throw new EmptySystemOrderIdException("Передан некорректный Id платежа в системе банка.");
                }

                var order = await _garantActionRepository.GetOrderByIdAsync(orderId);

                if (order == null)
                {
                    throw new ErrorUpdateSystemOrderIdException(orderId, "Ошибка при обновлении systemOrderId по параметру");
                }

                order.TinkoffSystemOrderId = systemOrderId;
                _postgreDbContext.Orders.Update(order);
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
    }
}
