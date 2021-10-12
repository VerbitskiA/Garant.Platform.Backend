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
        /// Метод получит список франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetFranchisesListAsync()
        {
            try
            {
                var result = await (from p in _postgreDbContext.Franchises
                                    select new FranchiseOutput
                                    {
                                        DateCreate = p.DateCreate,
                                        Price = string.Format("{0:0,0}", p.Price),
                                        CountDays = p.CountDays,
                                        DayDeclination = p.DayDeclination,
                                        Text = p.Text,
                                        TextDoPrice = p.TextDoPrice,
                                        Title = p.Title,
                                        Url = p.Url,
                                        FullText = p.Text + " " + p.CountDays + " " + p.DayDeclination
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
                    .Take(4)
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
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetFranchiseQuickSearchAsync()
        {
            try
            {
                var result = await (from p in _postgreDbContext.Franchises
                        select new FranchiseOutput
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
                    .Take(4)
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
        /// Метод получит список городов франшиз.
        /// </summary>
        /// <returns>Список городов.</returns>
        public async Task<IEnumerable<FranchiseCityOutput>> GetFranchisesCitiesListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.FranchiseCities
                                    select new FranchiseCityOutput
                                    {
                                        CityCode = c.CityCode,
                                        CityName = c.CityName
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
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<CategoryOutput>> GetFranchisesCategoriesListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.Categories
                                    select new CategoryOutput
                                    {
                                        CategoryCode = c.CategoryCode,
                                        CategoryName = c.CategoryName
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
        /// Метод получит список видов бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<ViewBusinessOutput>> GetFranchisesViewBusinessListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.ViewBusiness
                                    select new ViewBusinessOutput
                                    {
                                        ViewCode = c.ViewCode,
                                        ViewName = c.ViewName
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
    }
}
