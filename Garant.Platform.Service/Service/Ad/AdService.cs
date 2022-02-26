using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Ad.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Ad
{
    /// <summary>
    /// Сервис объявлений.
    /// </summary>
    public sealed class AdService : IAdService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public AdService()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
        }

        /// <summary>
        /// Метод получит список новых объявлений.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        public async Task<IEnumerable<AdOutput>> GetNewAdsAsync()
        {
            try
            {
                var commonService = AutoFac.Resolve<ICommonService>();

                //TODO: CountDays и DayDeclination сделать вычисляемыми и не хранить в БД.
                var result = await (from a in _postgreDbContext.Ads
                                    orderby a.DateCreate
                                    select new AdOutput
                                    {
                                        CountDays = a.CountDays,
                                        DateCreate = a.DateCreate,   
                                        DayDeclination = a.DayDeclination,
                                        Price = string.Format("{0:0,0}", a.Price),
                                        Text = a.Text,
                                        TextDoPrice = a.TextDoPrice,
                                        Title = a.Title,
                                        Url = a.Url
                                    })
                    .Reverse()
                    .Take(4)
                    .ToListAsync();

                //исправим склонение слова "день"
                foreach (var item in result)
                {
                    item.DayDeclination = await commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

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
