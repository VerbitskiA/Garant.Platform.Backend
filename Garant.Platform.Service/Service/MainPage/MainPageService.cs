using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Actions.Output;
using Garant.Platform.Models.Category.Output;
using Garant.Platform.Models.LastBuy.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Service.Service.MainPage
{
    /// <summary>
    /// Сервис главной страницы.
    /// </summary>
    public sealed class MainPageService : IMainPageService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public MainPageService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий бизнеса. Все это дело разбито на 4 столбца.</returns>
        public async Task<GetResultBusinessCategoryOutput> GetCategoriesListAsync()
        {
            try
            {
                var result = new GetResultBusinessCategoryOutput();

                // Получит весь список категорий.
                var allColList = await (from c in _postgreDbContext.BusinessCategories
                                        orderby c.Column
                                        select new BusinessCategoryOutput
                                        {
                                            Name = c.Name,
                                            Position = c.Position,
                                            Column = c.Column,
                                            Url = c.Url
                                        })
                    .ToListAsync();

                // Распределит по спискам в зависимости от столбца.
                foreach (var item in allColList)
                {
                    // Если все списки наполнены, то незачем продолжать дальше.
                    if (result.ResultCol1.Count >= 16 && result.ResultCol2.Count >= 16 && result.ResultCol3.Count >= 16 && result.ResultCol4.Count >= 16)
                    {
                        continue;
                    }

                    // Смотрит 1 столбец.
                    if (item.Column == 1)
                    {
                        // Если еще не было записей, то добавит первый и пропустит итерацию.
                        if (!result.ResultCol1.Any())
                        {
                            result.ResultCol1.Add(item);
                            continue;
                        }

                        // Если список еще не дошел до 16 записей, то продолжит заполнять.
                        if (result.ResultCol1.Count < 16)
                        {
                            result.ResultCol1.Add(item);
                        }

                        // Если уже 16 записей, то будет добавлять в следующий столбец.
                        else
                        {
                            if (result.ResultCol2.Count < 16)
                            {
                                result.ResultCol2.Add(item);
                            }
                        }
                    }

                    // Смотрит 2 столбец.
                    else if (item.Column == 2)
                    {
                        // Если еще не было записей, то добавит первый и пропустит итерацию.
                        if (!result.ResultCol2.Any())
                        {
                            result.ResultCol2.Add(item);
                            continue;
                        }

                        // Если список еще не дошел до 16 записей, то продолжит заполнять.
                        if (result.ResultCol2.Count < 16)
                        {
                            result.ResultCol2.Add(item);
                        }
                    }

                    // Смотрит 3 столбец.
                    else if (item.Column == 3)
                    {
                        // Если еще не было записей, то добавит первый и пропустит итерацию.
                        if (!result.ResultCol3.Any())
                        {
                            result.ResultCol3.Add(item);
                            continue;
                        }

                        // Если список еще не дошел до 16 записей, то продолжит заполнять.
                        if (result.ResultCol3.Count < 16)
                        {
                            result.ResultCol3.Add(item);
                        }

                        // Если уже 16 записей, то будет добавлять в следующий столбец.
                        else
                        {
                            if (result.ResultCol4.Count < 16)
                            {
                                result.ResultCol4.Add(item);
                            }
                        }
                    }

                    // Смотрит 4 столбец.
                    else if (item.Column == 4)
                    {
                        // Если еще не было записей, то добавит первый и пропустит итерацию.
                        if (!result.ResultCol4.Any())
                        {
                            result.ResultCol4.Add(item);
                            continue;
                        }

                        // Если список еще не дошел до 16 записей, то продолжит заполнять.
                        if (result.ResultCol4.Count < 16)
                        {
                            result.ResultCol4.Add(item);
                        }
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
                                        Title = a.Title
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
    }
}
