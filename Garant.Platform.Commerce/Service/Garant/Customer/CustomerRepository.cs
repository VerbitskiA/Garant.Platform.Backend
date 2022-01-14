using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Commerce.Output;
using Garant.Platform.Models.Entities.Commerce;
using Garant.Platform.Models.Entities.Document;
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
        /// <param name="description">Объект с описанием платежа.</param>
        /// <param name="iteration">Номер итерации этапа.</param>
        /// <returns>Данные платежа.</returns>
        public async Task<OrderOutput> CreateOrderAsync(long originalId, double amount, Description description, string orderType, string userId, int iteration)
        {
            try
            {
                //long lastId = 1;
                var optionalType = "DocumentCustomerAct" + iteration;

                //if (await _postgreDbContext.Orders.AnyAsync())
                //{
                //    lastId = await _postgreDbContext.Orders.MaxAsync(o => o.OrderId);
                //}

                //lastId++;

                // Проверит существование такого заказа.
                var checkOrder = await _postgreDbContext.Orders
                    .AsNoTracking()
                    .Where(o => o.OriginalId == originalId
                                && o.UserId.Equals(userId)
                                && o.OrderType.Equals(orderType)
                                && o.Iteration == iteration
                                && o.OptionalType.Equals(optionalType))
                    .FirstOrDefaultAsync();

                if (checkOrder == null)
                {
                    // Создаст новый заказ.
                    await _postgreDbContext.Orders.AddAsync(new OrderEntity
                    {
                        //OrderId = lastId,
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
                        UserId = userId,
                        Iteration = iteration,
                        OptionalType = optionalType
                    });

                    await _postgreDbContext.SaveChangesAsync();
                }

                // Обновит заказ.
                else
                {
                    var getOrder = await _postgreDbContext.Orders
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.OriginalId == originalId
                                                  && o.UserId.Equals(userId)
                                                  && o.OrderType.Equals(orderType));

                    if (getOrder != null)
                    {
                        var newOrder = new OrderEntity
                        {
                            OrderId = getOrder.OrderId,
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
                            UserId = userId,
                            Iteration = iteration,
                            OptionalType = optionalType
                        };

                        _postgreDbContext.Orders.Update(newOrder);
                        await _postgreDbContext.SaveChangesAsync();
                    }
                }

                var result = await _postgreDbContext.Orders
                    .OrderByDescending(o => o.OrderId)
                    .Select(o => new OrderOutput { OrderId = o.OrderId })
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
        /// Метод проставит флаг оплаты в true документам покупателя, которые оплачены.
        /// </summary>
        /// <param name="userId">Id покупателя.</param>
        /// <param name="iteration">Номер итерации.</param>
        public async Task SetDocumentsCustomerPaymentAsync(string userId, int iteration)
        {
            try
            {
                var documents = await _postgreDbContext.Documents.AsNoTracking()
                    .Where(d => d.UserId.Equals(userId)
                        && d.IsDealDocument
                        && d.IsSend == true
                        && d.IsApproveDocument == true
                        && d.IsRejectDocument == false
                        && d.IsPay == false)
                    .ToListAsync();

                long updateDocumentId = 0;

                foreach (var item in documents)
                {
                    var value = 0;

                    // Возьмет только число из строки.
                    int.TryParse(string.Join("", item.DocumentType.Where(char.IsDigit)), out value);

                    // Если найдет нужный акт, то обновит его оплату.
                    if (value == 0 && item.DocumentType.Equals("DocumentCustomer") || value == iteration)
                    {
                        updateDocumentId = item.DocumentId;
                    }
                }

                // Обновит документу флаг оплаты.
                if (updateDocumentId > 0)
                {
                    var updateDocument = await _postgreDbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == updateDocumentId);

                    if (updateDocument != null)
                    {
                        updateDocument.IsPay = true;
                        _postgreDbContext.Documents.Update(updateDocument);
                        await _postgreDbContext.SaveChangesAsync();
                    }
                }

                Console.WriteLine();
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
