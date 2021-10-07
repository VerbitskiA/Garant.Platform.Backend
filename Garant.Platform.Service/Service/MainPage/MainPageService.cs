using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Category.Output;
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
        /// <returns>Список категорий бизнеса.</returns>
        public async Task<IEnumerable<BusinessCategoryOutput>> GetCategoriesListAsync()
        {
            try
            {
                var result = await (from c in _postgreDbContext.BusinessCategories
                                    orderby c.Column
                                    select new BusinessCategoryOutput
                                    {
                                        Name = c.Name,
                                        Position = c.Position,
                                        Column = c.Column,
                                        Url = c.Url
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
