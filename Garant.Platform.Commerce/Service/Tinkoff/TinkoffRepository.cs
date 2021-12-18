using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
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

        public TinkoffRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
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
    }
}
