namespace Garant.Platform.Messaging.Models.Chat.Input
{
    /// <summary>
    /// Класс входной модели чата.
    /// </summary>
    public class ChatInput
    {
        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Id диалога.
        /// </summary>
        public long DialogId { get; set; }
    }
}
