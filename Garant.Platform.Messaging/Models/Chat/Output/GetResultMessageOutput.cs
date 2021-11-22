using System.Collections.Generic;

namespace Garant.Platform.Messaging.Models.Chat.Output
{
    /// <summary>
    /// Класс выходной модели списка сообщений диалога.
    /// </summary>
    public class GetResultMessageOutput
    {
        /// <summary>
        /// Список сообщений.
        /// </summary>
        public List<MessageOutput> Messages { get; set; } = new List<MessageOutput>();

        /// <summary>
        /// Кол-во сообщений.
        /// </summary>
        public long Count => Messages.Count;

        /// <summary>
        /// Состояние диалога.
        /// </summary>
        public string DialogState { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Название предмета чата.
        /// </summary>
        public string ChatItemName { get; set; }
    }
}
