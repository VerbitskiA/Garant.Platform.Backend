using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
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
    }
}
