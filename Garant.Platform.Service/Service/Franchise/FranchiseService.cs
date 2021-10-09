using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Franchise.Output;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Service.Service.Franchise
{
    public sealed class FranchiseService : IFranchiseService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public FranchiseService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод получит список популярных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<PopularFranchiseOutput>> GetPopularFranchises()
        {
            try
            {
                var result = await (from p in _postgreDbContext.PopularFranchises
                                    select new PopularFranchiseOutput
                                    {
                                        DateCreate = p.DateCreate,
                                        Price = string.Format("{0:0,0}", p.Price),
                                        CountDays = p.CountDays,
                                        DayDeclination = p.DayDeclination,
                                        Text = p.Text,
                                        TextDoPrice = p.TextDoPrice,
                                        Title = p.Title,
                                        Url = p.Url
                                    })
                    .ToListAsync();

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
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchises()
        {
            try
            {
                var result = await (from p in _postgreDbContext.PopularFranchises
                                    select new PopularFranchiseOutput
                                    {
                                        DateCreate = p.DateCreate,
                                        Price = string.Format("{0:0,0}", p.Price),
                                        CountDays = p.CountDays,
                                        DayDeclination = p.DayDeclination,
                                        Text = p.Text,
                                        TextDoPrice = p.TextDoPrice,
                                        Title = p.Title,
                                        Url = p.Url
                                    })
                    .Take(5)
                    .ToListAsync();

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
