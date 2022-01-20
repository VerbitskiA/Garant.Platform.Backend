using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Messaging.Models.Chat.Output;

namespace Garant.Platform.Messaging.Abstraction.Chat
{
    /// <summary>
    /// Абстракция сервиса чата.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Метод отправит сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="dialogId">Id диалога.</param>
        /// <returns>Список сообщений.</returns>
        Task<GetResultMessageOutput> SendMessageAsync(string message, string account, long dialogId);

        /// <summary>
        /// Метод получит диалог или создаст новый.
        /// </summary>
        /// <param name="dialogId">Id диалога.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="ownerId">Id владельца/представителя. Пользователя, который владеет бизнесом или франшизей.</param>
        /// <param name="typeItem">Тип предмета.</param>
        /// <returns>Список сообщений.</returns>
        Task<GetResultMessageOutput> GetDialogAsync(long? dialogId, string account, string ownerId, string typeItem);

        /// <summary>
        /// Метод получит список диалогов пользователя.
        /// </summary>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Список диалогов.</returns>
        Task<IEnumerable<DialogOutput>> GetDialogsAsync(string account);
    }
}
