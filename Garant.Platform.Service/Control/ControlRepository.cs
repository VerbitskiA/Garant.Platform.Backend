using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Control;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Control.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Control
{
    /// <summary>
    /// КЛасс реализует методы репозитория контролов.
    /// </summary>
    public class ControlRepository : IControlRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public ControlRepository()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
        }

        /// <summary>
        /// Метод получит список названий банков для профиля.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Список названий банков.</returns>
        public async Task<IEnumerable<ControlOutput>> GetFilterBankNameValuesAsync(string userId)
        {
            try
            {
                // Найдет значение по дефолту у списка банков.
                var defaultSelectedCode = await _postgreDbContext.UsersInformation
                    .Where(u => u.UserId.Equals(userId))
                    .Select(u => u.DefaultBankName)
                    .FirstOrDefaultAsync();

                var result = await _postgreDbContext.Controls
                    .OrderBy(c => c.Position)
                    .Select(c => new ControlOutput
                    {
                        ControlName = c.ControlName,
                        ControlType = c.ControlType,
                        Position = c.Position,
                        ValueId = c.ValueId,
                        Value = c.Value,
                        IsDefault = c.IsDefault
                    })
                    .ToListAsync();

                // Проставит записи значение по дефолту, если она есть.
                if (!string.IsNullOrEmpty(defaultSelectedCode))
                {
                    foreach (var item in result.Where(i => i.Value.ToString().Equals(defaultSelectedCode)))
                    {
                        item.IsDefault = true;
                    }
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
        /// Метод найдет банки по их названию.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список названий банков.</returns>
        public async Task<IEnumerable<ControlOutput>> SearchFilterBankNameValueAsync(string searchText)
        {
            try
            {
                var result = await _postgreDbContext.Controls
                    .Where(c => c.Value.ToLower().Contains(searchText.ToLower()))
                    .OrderBy(c => c.Position)
                    .Select(c => new ControlOutput
                    {
                        ControlName = c.ControlName,
                        ControlType = c.ControlType,
                        Position = c.Position,
                        ValueId = c.ValueId,
                        Value = c.Value,
                        IsDefault = c.IsDefault
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
