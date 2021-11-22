namespace Garant.Platform.Messaging.Models.Chat.Input
{
    /// <summary>
    /// Класс входной модели для диалога.
    /// </summary>
    public class DialogInput
    {
        /// <summary>
        /// Id диалога.
        /// </summary>
        public long? DialogId { get; set; }

        /// <summary>
        /// Тип предмета (т.е франшиза или бизнес).
        /// </summary>
        public string TypeItem { get; set; }

        /// <summary>
        /// Id владельца/представителя.
        /// </summary>
        public string OwnerId { get; set; }
    }
}
