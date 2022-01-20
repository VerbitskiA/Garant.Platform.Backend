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
        /// Полное имя пользователя.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Название предмета чата.
        /// </summary>
        public string ChatItemName { get; set; }

        /// <summary>
        /// Дата начала диалога.
        /// </summary>
        public string DateStartDialog { get; set; }

        /// <summary>
        /// Id диалога.
        /// </summary>
        public long DialogId { get; set; }

        /// <summary>
        /// Путь к изображению предмета обсуждения.
        /// </summary>
        public string Url { get; set; }
    }
}
