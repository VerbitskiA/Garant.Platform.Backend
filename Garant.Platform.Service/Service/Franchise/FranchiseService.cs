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
                                        FullText = p.Text + " " + p.CountDays + " " + p.DayDeclination,
                                        IsGarant = p.IsGarant,
                                        ProfitPrice = p.ProfitPrice
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
                            Url = p.Url,
                            IsGarant = p.IsGarant,
                            ProfitPrice = p.ProfitPrice
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

        /// <summary>
        /// Метод фильтрации франшиз по разным атрибутам.
        /// </summary>
        /// <param name="typeSort">Тип фильтрации цены (по возрастанию или убыванию).</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <param name="minPrice">Прибыль от.</param>
        /// <param name="maxPrice">Прибыль до.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        public async Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string typeSort, string minPrice, string maxPrice, bool isGarant = false)
        {
            try
            {
                IEnumerable<FranchiseOutput> result = null;

                // Сортировать на возрастанию цены.
                if (typeSort.Equals("Asc"))
                {
                    var query = (from f in _postgreDbContext.Franchises
                                 orderby f.Price
                                 select new FranchiseOutput
                                 {
                                     DateCreate = f.DateCreate,
                                     Price = string.Format("{0:0,0}", f.Price),
                                     CountDays = f.CountDays,
                                     DayDeclination = f.DayDeclination,
                                     Text = f.Text,
                                     TextDoPrice = f.TextDoPrice,
                                     Title = f.Title,
                                     Url = f.Url,
                                     FullText = f.Text + " " + f.CountDays + " " + f.DayDeclination,
                                     IsGarant = f.IsGarant,
                                     ProfitPrice = f.ProfitPrice
                                 })
                        .AsQueryable();

                    // Нужно ли дополнить запрос для сортировки по прибыли.
                    if (!string.IsNullOrEmpty(minPrice) && !string.IsNullOrEmpty(maxPrice))
                    {
                        query = query.Where(c => c.ProfitPrice <= Convert.ToDouble(maxPrice) 
                                                 && c.ProfitPrice >= Convert.ToDouble(minPrice));
                    }

                    result = await query.ToListAsync();
                }

                // Сортировать на убыванию цены.
                else if (typeSort.Equals("Desc"))
                {
                    var query = (from f in _postgreDbContext.Franchises
                                 orderby f.Price descending
                                 select new FranchiseOutput
                                 {
                                     DateCreate = f.DateCreate,
                                     Price = string.Format("{0:0,0}", f.Price),
                                     CountDays = f.CountDays,
                                     DayDeclination = f.DayDeclination,
                                     Text = f.Text,
                                     TextDoPrice = f.TextDoPrice,
                                     Title = f.Title,
                                     Url = f.Url,
                                     FullText = f.Text + " " + f.CountDays + " " + f.DayDeclination,
                                     IsGarant = f.IsGarant,
                                     ProfitPrice = f.ProfitPrice
                                 })
                        .AsQueryable();

                    // Нужно ли дополнить запрос для сортировки по прибыли.
                    if (!string.IsNullOrEmpty(minPrice) && !string.IsNullOrEmpty(maxPrice))
                    {
                        query = query.Where(c => c.ProfitPrice <= Convert.ToDouble(maxPrice)
                                                 && c.ProfitPrice >= Convert.ToDouble(minPrice));
                    }

                    result = await query.ToListAsync();
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

        /// <summary>
        /// Метод получит новые франшизы, которые были созданы в текущем месяце.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetNewFranchisesAsync()
        {
            try
            {
                var year = DateTime.Now.Year;

                var result = await (from f in _postgreDbContext.Franchises
                                    where f.DateCreate.Year == year
                                    select new FranchiseOutput
                                    {
                                        DateCreate = f.DateCreate,
                                        Price = string.Format("{0:0,0}", f.Price),
                                        CountDays = f.CountDays,
                                        DayDeclination = f.DayDeclination,
                                        Text = f.Text,
                                        TextDoPrice = f.TextDoPrice,
                                        Title = f.Title,
                                        Url = f.Url,
                                        FullText = f.Text + " " + f.CountDays + " " + f.DayDeclination,
                                        IsGarant = f.IsGarant,
                                        ProfitPrice = f.ProfitPrice
                                    })
                    .Take(10)
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
        /// Метод получит список отзывов о франшизах.
        /// </summary>
        /// <returns>Список отзывов.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetReviewsFranchisesAsync()
        {
            try
            {
                var result = await (from f in _postgreDbContext.Franchises
                                    select new FranchiseOutput
                                    {
                                        DateCreate = f.DateCreate,
                                        Price = string.Format("{0:0,0}", f.Price),
                                        CountDays = f.CountDays,
                                        DayDeclination = f.DayDeclination,
                                        Text = f.Text,
                                        TextDoPrice = f.TextDoPrice,
                                        Title = f.Title,
                                        Url = f.Url,
                                        FullText = f.Text + " " + f.CountDays + " " + f.DayDeclination,
                                        IsGarant = f.IsGarant,
                                        ProfitPrice = f.ProfitPrice
                                    })
                    .Take(10)
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
