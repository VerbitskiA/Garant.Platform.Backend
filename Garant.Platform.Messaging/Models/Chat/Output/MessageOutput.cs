using System.Text.Json.Serialization;

namespace Garant.Platform.Messaging.Models.Chat.Output
{
    /// <summary>
    /// Класс выходной модели для сообщений диалога.
    /// </summary>
    public class MessageOutput
    {
        /// <summary>
        /// Текст сообщения.
        /// </summary>
        //[JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Id диалога, к которому принадлежит сообщение.
        /// </summary>
        //[JsonPropertyName("dialogId")]
        public long? DialogId { get; set; }

        /// <summary>
        /// Дата написания сообщения.
        /// </summary>
        //[JsonPropertyName("created")]
        public string Created { get; set; }

        /// <summary>
        /// Id пользователя, которому принадлежит сообщение.
        /// </summary>        
        //[JsonPropertyName("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Флаг принадлежности сообщения текущему пользователю.
        /// </summary>
        //[JsonPropertyName("isMyMessage")]
        public bool IsMyMessage { get; set; }
    }
}
