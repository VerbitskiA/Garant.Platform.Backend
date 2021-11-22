using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Messaging.Models.Chat.Output;

namespace Garant.Platform.Messaging.Abstraction.Chat
{
    /// <summary>
    /// Абстракция репозитория для работы чата с БД.
    /// </summary>
    public interface IChatRepository
    {
        /// <summary>
        /// Метод проверит существование диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Флаг проверки.</returns>
        Task<bool> CheckDialogAsync(long dialogId);

        /// <summary>
        /// Метод сохранит сообщение в БД.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="dialogId">Id диалога.</param>
        /// <param name="dateCreated">Дата записи сообщения.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="isMyMessage">Флаг принадлежности сообщения пользователю, который пишет сообщение.</param>
        Task SaveMessageAsync(string message, long dialogId, DateTime dateCreated, string userId, bool isMyMessage);

        /// <summary>
        /// Метод получит список сообщений диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Список сообщений.</returns>
        Task<List<MessageOutput>> GetMessagesAsync(long dialogId);

        /// <summary>
        /// Метод найдет Id диалога в участниках диалога.
        /// </summary>
        /// <param name="id">Id пользователя.</param>
        /// <returns>Id диалога.</returns>
        Task<long> FindDialogIdAsync(string id);

        /// <summary>
        /// Метод создаст новый диалог.
        /// </summary>
        /// <param name="dialogName">Название диалога.</param>
        /// <param name="dateCreated">Дата создания диалога.</param>
        Task CreateDialogAsync(string dialogName, DateTime dateCreated);

        /// <summary>
        /// Метод получит Id последнего диалога, который был создан.
        /// </summary>
        /// <returns>Id диалога.</returns>
        Task<long> GetLastDialogIdAsync();

        /// <summary>
        /// Метод добавит текущего пользователя и представителя/владельца к диалогу.
        /// </summary>
        /// <param name="userId">Id текущего пользователя.</param>
        /// <param name="ownerId">Id представителя/владельца</param>
        /// <param name="newDialogId">Id нового диалога.</param>
        Task AddDialogMembersAsync(string userId, string ownerId, long newDialogId);

        /// <summary>
        /// Метод получит участников диалога по DialogId.
        /// <param name="dialogId">Id диалога, участников которого нужно получить.</param>
        /// </summary>
        /// <returns>Список участников.</returns>
        Task<IEnumerable<string>> GetDialogMembersAsync(long dialogId);

        /// <summary>
        /// Метод получит все диалогы.
        /// </summary>
        /// <param name="userId">Id текущего пользователя, для которого нужно получить список диалогов.</param>
        /// <returns>Список диалогов.</returns>
        Task<IEnumerable<DialogOutput>> GetDialogsAsync(string userId);

        /// <summary>
        /// Метод найдет последнее сообщение диалога.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Последнее сообщение.</returns>
        Task<string> GetLastMessageAsync(long dialogId);
    }
}
