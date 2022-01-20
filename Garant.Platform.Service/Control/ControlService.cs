using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Control;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Control.Output;

namespace Garant.Platform.Services.Control
{
    /// <summary>
    /// Класс реализует методы сервиса контролов.
    /// </summary>
    public sealed class ControlService : IControlService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IControlRepository _controlRepository;
        private readonly IUserRepository _userRepository;

        public ControlService(PostgreDbContext postgreDbContext, IControlRepository controlRepository, IUserRepository userRepository)
        {
            _postgreDbContext = postgreDbContext;
            _controlRepository = controlRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Метод получит список названий банков для профиля.
        /// </summary>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Список названий банков.</returns>
        public async Task<IEnumerable<ControlOutput>> GetFilterBankNameValuesAsync(string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                var result = await _controlRepository.GetFilterBankNameValuesAsync(userId);

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
                if (string.IsNullOrEmpty(searchText))
                {
                    return new List<ControlOutput>();
                }

                var result = await _controlRepository.SearchFilterBankNameValueAsync(searchText);

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
