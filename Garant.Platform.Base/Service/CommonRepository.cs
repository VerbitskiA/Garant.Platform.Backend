using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Base.Service
{
    /// <summary>
    /// Класс реализует методы общего репозитория.
    /// </summary>
    public class CommonRepository : ICommonRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        
        public CommonRepository()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
        }

        /// <summary>
        /// Метод получит ссылку для формирования.
        /// </summary>
        /// <param name="cardType">Тип карточки.</param>
        /// <returns>Строка url.</returns>
        public async Task<string> GetCardUrlAsync(string cardType)
        {
            try
            {
                if (string.IsNullOrEmpty(cardType))
                {
                    throw new EmptyCardTypeException(cardType);
                }

                var url = await _postgreDbContext.ReturnUrls
                    .Where(u => u.TypeLink.Equals(cardType))
                    .Select(u => u.Link)
                    .FirstOrDefaultAsync();

                return url;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}