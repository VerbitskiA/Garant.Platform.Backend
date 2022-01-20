using System;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Models.Entities.Logger;

namespace Garant.Platform.Core.Logger
{
    /// <summary>
    /// Класс логирования ошибок в БД.
    /// </summary>
    public class Logger
    {
        private readonly LoggerEntity _logger;
        private readonly PostgreDbContext _postgreDbContext;

        public Logger(PostgreDbContext postgreDbContext, string typeException, string exception, string stackTrace)
        {
            /// <summary>
            /// Инициализация объекта логера.
            /// </summary>
            _logger = new LoggerEntity
            {
                TypeException = typeException,
                Exception = exception,
                StackTrace = stackTrace,
                Date = DateTime.Now
            };

            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод пишет лог в базу с типом Critical.
        /// </summary>
        public async Task LogCritical()
        {
            _logger.LogLvl = "Critical";
            await _postgreDbContext.Logs.AddAsync(_logger);
            await _postgreDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Метод пишет лог в базу с типом Information.
        /// </summary>
        public async Task LogInformation()
        {
            _logger.LogLvl = "Information";
            await _postgreDbContext.Logs.AddAsync(_logger);
            await _postgreDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Метод пишет лог в базу с типом Error.
        /// </summary>
        public async Task LogError()
        {
            _logger.LogLvl = "Error";
            await _postgreDbContext.Logs.AddAsync(_logger);
            await _postgreDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Метод пишет лог в базу с типом Warning.
        /// </summary>
        public async Task LogWarning()
        {
            _logger.LogLvl = "Warning";
            await _postgreDbContext.Logs.AddAsync(_logger);
            await _postgreDbContext.SaveChangesAsync();
        }
    }
}
