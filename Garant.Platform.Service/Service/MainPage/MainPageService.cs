using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Abstractions.MainPage;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Actions.Output;
using Garant.Platform.Models.Category.Output;
using Garant.Platform.Models.Franchise.Output;
using Garant.Platform.Models.LastBuy.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.MainPage
{
    /// <summary>
    /// Сервис главной страницы.
    /// </summary>
    public sealed class MainPageService : IMainPageService
    {
        private readonly PostgreDbContext _postgreDbContext;
        
        public MainPageService()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий бизнеса. Все это дело разбито на 4 столбца.</returns>
        public async Task<GetResultCategoryOutput> GetCategoriesListAsync()
        {
            try
            {
                var result = new GetResultCategoryOutput();

                // Получит весь список категорий бизнеса.
                var allColBusinessList = await (from c in _postgreDbContext.BusinessCategories
                                        orderby c.Position
                                        select new BusinessCategoryOutput
                                        {
                                            Name = c.BusinessCategoryName,
                                            Position = c.Position,
                                            Column = c.Column,
                                            Url = c.Url
                                        })
                    .ToListAsync();
                
                // Получит весь список категорий франшиз.
                var allColFranchiseList = await (from c in _postgreDbContext.FranchiseCategories
                        orderby c.Position
                        select new FranchiseCategoryOutput
                        {
                            Name = c.FranchiseCategoryName,
                            Position = c.Position,
                            Column = c.Column,
                            Url = c.Url
                        })
                    .ToListAsync(); 

                // Заполнит категориями бизнеса 1 и 2 столбцы.
                foreach (var item in allColBusinessList)
                {
                    // Если все списки наполнены, то незачем продолжать дальше.
                    if (result.ResultCol1.Count >= 16 && result.ResultCol2.Count >= 16)
                    {
                        continue;
                    }

                    // Заполнит первый столбец.
                    if (result.ResultCol1.Count <= 16)
                    {
                        result.ResultCol1.Add(item);
                    }

                    // Заполнит второй столбец.
                    else
                    {
                        result.ResultCol2.Add(item);
                    }
                }
                
                // Заполнит категориями франшиз 1 и 2 столбцы.
                foreach (var item in allColFranchiseList)
                {
                    if (result.ResultCol3.Count >= 16 && result.ResultCol4.Count >= 16)
                    {
                        continue;
                    }

                    // Заполнит первый столбец.
                    if (result.ResultCol3.Count <= 16)
                    {
                        result.ResultCol3.Add(item);
                    }

                    // Заполнит второй столбец.
                    else
                    {
                        result.ResultCol4.Add(item);
                    }
                }

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
        /// Метод получит последние 5 записей недавно купленных франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<LastBuyOutput>> GetSliderLastBuyAsync()
        {
            try
            {
                var commonService = AutoFac.Resolve<ICommonService>();

                // TODO: переделать CountDays по аналогии как сделано у франшиз. Это поле должно быть вычисляемым и не храниться в БД. CountDays и DayDeclination убрать из БД а тут сделать CountDays вычисляемым.
                var result = await (from res in _postgreDbContext.LastBuys
                                    select new LastBuyOutput
                                    {
                                        CountDays = res.CountDays,
                                        DateBuy = res.DateBuy,    
                                        DayDeclination = res.DayDeclination,
                                        Name = res.Name,
                                        Price = string.Format("{0:0,0}", res.Price),
                                        Text = res.Text,
                                        TextDoPrice = res.TextDoPrice,
                                        Url = res.Url
                                    })
                    .Take(5)
                    .ToListAsync();

                //исправляем склонение слова "день"
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
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит данные для блока событий главной страницы.
        /// </summary>
        /// <returns>Список данных.</returns>
        public async Task<IEnumerable<MainPageActionOutput>> GetActionsMainPageAsync()
        {
            try
            {
                var result = await (from a in _postgreDbContext.MainPageActions
                                    select new MainPageActionOutput
                                    {
                                        ButtonText = a.ButtonText,
                                        SubTitle = a.SubTitle,
                                        Text = a.Text,
                                        Title = a.Title,
                                        IsTop = a.IsTop
                                    })
                    .ToListAsync();

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
        /// Метод получит список франшиз на основе фильтров.
        /// </summary>
        /// <param name="viewBusinessCode">Код вида бизнеса.</param>
        /// <param name="categoryCode">Код категории.</param>
        /// <param name="minPrice">Цена от.</param>
        /// <param name="maxPrice">Цена до.</param>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string viewBusinessCode, string categoryCode, double minPrice, double maxPrice)
        {
            try
            {
                var commonService = AutoFac.Resolve<ICommonService>();

                var result = await (from f in _postgreDbContext.Franchises
                                    where f.ViewBusiness.Equals(viewBusinessCode)
                                        && f.Category.Equals(categoryCode)
                                        && (f.Price <= maxPrice && f.Price >= minPrice)
                                    orderby f.FranchiseId
                                    select new FranchiseOutput
                                    {
                                        Category = f.Category,
                                        CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                        DayDeclination = "дня",
                                        DateCreate = f.DateCreate,
                                        Price = string.Format("{0:0,0}", f.Price),
                                        TextDoPrice = f.TextDoPrice,
                                        Text = f.Text,
                                        SubCategory = f.SubCategory,
                                        Title = f.Title,
                                        Url = f.Url
                                    })
                    .Take(4)
                    .ToListAsync();

                foreach (var item in result)
                {
                    //исправляем склонение слова "день"
                    item.DayDeclination = await commonService.GetCorrectDayDeclinationAsync(item.CountDays);

                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

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
