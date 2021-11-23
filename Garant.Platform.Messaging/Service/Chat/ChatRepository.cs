using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Messaging.Abstraction.Chat;
using Garant.Platform.Messaging.Models.Chat.Output;
using Garant.Platform.Models.Entities.Chat;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Messaging.Service.Chat
{
    /// <summary>
    /// Репозиторий чата для работы с БД.
    /// </summary>
    public sealed class ChatRepository : IChatRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public ChatRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод проверит существование диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> CheckDialogAsync(long dialogId)
        {
            try
            {
                // Проверит существование диалога.
                var isDialog = await _postgreDbContext.MainInfoDialogs
                    .Where(d => d.DialogId == dialogId)
                    .FirstOrDefaultAsync();

                return isDialog != null;
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
        /// Метод сохранит сообщение в БД.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="dialogId">Id диалога.</param>
        /// <param name="dateCreated">Дата записи сообщения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="isMyMessage">Флаг принадлежности сообщения пользователю, который пишет сообщение.</param>
        public async Task SaveMessageAsync(string message, long dialogId, DateTime dateCreated, string userId, bool isMyMessage)
        {
            try
            {
                // Запишет сообщение в диалог.
                await _postgreDbContext.DialogMessages.AddAsync(new DialogMessageEntity
                {
                    Message = message,
                    DialogId = dialogId,
                    Created = dateCreated,
                    UserId = userId,
                    IsMyMessage = isMyMessage
                });

                await _postgreDbContext.SaveChangesAsync();
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
        /// Метод получит список сообщений диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Список сообщений.</returns>
        public async Task<List<MessageOutput>> GetMessagesAsync(long dialogId)
        {
            try
            {
                var messages = await _postgreDbContext.DialogMessages
                    .Where(d => d.DialogId == dialogId)
                    .OrderBy(m => m.Created)
                    .Select(res => new MessageOutput
                    {
                        DialogId = res.DialogId,
                        Message = res.Message,
                        Created = string.Format("{0:f}", res.Created),
                        UserId = res.UserId,
                        IsMyMessage = res.IsMyMessage
                    })
                    .ToListAsync();

                return messages;
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
        /// Метод найдет Id диалога в участниках диалога.
        /// </summary>
        /// <param name="id">Id пользователя.</param>
        /// <returns>Id диалога.</returns>
        public async Task<long> FindDialogIdAsync(string id)
        {
            try
            {
                var result = await _postgreDbContext.DialogMembers
                    .Where(d => d.Id.Equals(id))
                    .Select(res => res.DialogId)
                    .FirstOrDefaultAsync();

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
        /// Метод создаст новый диалог.
        /// </summary>
        /// <param name="dialogName">Название диалога.</param>
        /// <param name="dateCreated">Дата создания диалога.</param>
        public async Task CreateDialogAsync(string dialogName, DateTime dateCreated)
        {
            try
            {
                long lastDialogId = 1000000;

                if (await _postgreDbContext.MainInfoDialogs.AnyAsync())
                {
                    // Найдет последний диалог.
                    lastDialogId = await _postgreDbContext.MainInfoDialogs.MaxAsync(d => d.DialogId);
                    lastDialogId++;
                }

                await _postgreDbContext.MainInfoDialogs.AddAsync(new MainInfoDialogEntity
                {
                    DialogId = lastDialogId,
                    DialogName = dialogName,
                    Created = dateCreated
                });

                await _postgreDbContext.SaveChangesAsync();
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
        /// Метод получит Id последнего диалога, который был создан.
        /// </summary>
        /// <returns>Id диалога.</returns>
        public async Task<long> GetLastDialogIdAsync()
        {
            try
            {
                var lastDialogId = await _postgreDbContext.MainInfoDialogs
                    .OrderBy(d => d.DialogId)
                    .Select(d => d.DialogId)
                    .LastOrDefaultAsync();

                return lastDialogId;
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
        /// Метод добавит текущего пользователя и представителя/владельца к диалогу.
        /// </summary>
        /// <param name="userId">Id текущего пользователя.</param>
        /// <param name="ownerId">Id представителя/владельца</param>
        /// <param name="newDialogId">Id нового диалога.</param>
        public async Task AddDialogMembersAsync(string userId, string ownerId, long newDialogId)
        {
            try
            {
                await _postgreDbContext.DialogMembers.AddRangeAsync(
                    new DialogMemberEntity
                    {
                        DialogId = newDialogId,
                        Id = userId,
                        Joined = DateTime.Now
                    },
                    new DialogMemberEntity
                    {
                        DialogId = newDialogId,
                        Id = ownerId,
                        Joined = DateTime.Now
                    });

                await _postgreDbContext.SaveChangesAsync();
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
        /// Метод получит участников диалога по DialogId.
        /// <param name="dialogId">Id диалога, участников которого нужно получить.</param>
        /// </summary>
        /// <returns>Список участников.</returns>
        public async Task<IEnumerable<string>> GetDialogMembersAsync(long dialogId)
        {
            try
            {
                var result = await _postgreDbContext.DialogMembers
                    .Where(d => d.DialogId == dialogId)
                    .Select(res => res.Id)
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
        /// Метод получит все диалогы.
        /// </summary>
        /// <param name="userId">Id текущего пользователя, для которого нужно получить список диалогов.</param>
        /// <returns>Список диалогов.</returns>
        public async Task<IEnumerable<DialogOutput>> GetDialogsAsync(string userId)
        {
            try
            {
                var dialogs = await (from dm in _postgreDbContext.DialogMembers
                                     join id in _postgreDbContext.MainInfoDialogs
                                         on dm.DialogId
                                         equals id.DialogId
                                     where dm.Id.Equals(userId)
                                     select new DialogOutput
                                     {
                                         DialogId = dm.DialogId,
                                         DialogName = id.DialogName,
                                         UserId = dm.Id,
                                         Created = id.Created.ToString(CultureInfo.CurrentCulture)
                                     })
                    .ToListAsync();

                return dialogs;
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
        /// Метод найдет последнее сообщение диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Последнее сообщение.</returns>
        public async Task<string> GetLastMessageAsync(long dialogId)
        {
            try
            {
                var lastMessage = await _postgreDbContext.DialogMessages
                    .Where(d => d.DialogId == dialogId)
                    .OrderBy(o => o.Created)
                    .Select(m => m.Message)
                    .LastOrDefaultAsync();

                return lastMessage;
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
        /// Метод получит дату начала диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Дата начала диалога.</returns>
        public async Task<string> GetDialogStartDate(long dialogId)
        {
            try
            {
                // Найдет дату начала диалога.
                var result = await _postgreDbContext.MainInfoDialogs
                    .Where(d => d.DialogId == dialogId)
                    .Select(d => d.Created.ToString("f"))
                    .FirstOrDefaultAsync();

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
