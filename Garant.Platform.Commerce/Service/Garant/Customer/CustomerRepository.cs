using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Commerce.Output;
using Garant.Platform.Models.Entities.Commerce;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    /// <summary>
    /// Сервис реализует методы репозитория Гаранта со стороны покупателя для работы с БД.
    /// </summary>
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public CustomerRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод создаст новый заказ.
        /// </summary>
        /// <param name="amount">Цена.</param>
        /// <param name="endDate">Дата окончания холдирования.</param>
        /// <param name="description">Объект с описанием платежа.</param>
        /// <param name="redirectUrl">Url редиректа после успешного платежа.</param>
        /// <returns>Данные платежа.</returns>
        public async Task<OrderOutput> CreateOrderAsync(long originalId, double amount, DateTime endDate, Description description, string redirectUrl, string orderType, string userId)
        {
            try
            {
                const int id = 1000000;
                var lastId = await _postgreDbContext.Orders.MaxAsync(o => o.OrderId);

                if (lastId <= 0)
                {
                    lastId = id;
                }

                lastId++;
                var last = lastId;

                await _postgreDbContext.Orders.AddAsync(new OrderEntity
                {
                    OrderId = last,
                    Amount = amount,
                    Currency = "RUB",
                    DateCreate = DateTime.Now,
                    ShortOrderName = description.Short,
                    FullOrderName = description.Full,
                    OrderStatus = "Hold",
                    TotalAmount = amount,
                    OrderType = orderType,
                    OriginalId = originalId,
                    ProductCount = 1,
                    UserId = userId
                });

                await _postgreDbContext.SaveChangesAsync();

                var lastOrderId = _postgreDbContext.Orders
                    .OrderByDescending(o => o.OrderId)
                    .Select(o => o.OrderId)
                    .AsQueryable();

                var lastOrder = await lastOrderId.FirstOrDefaultAsync();

                var result = new OrderOutput
                {
                    OrderId = lastOrder
                };

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
    }
}
