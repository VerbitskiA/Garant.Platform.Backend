namespace Garant.Platform.Models.Request.Output
{
    /// <summary>
    /// Класс выходной модели заявок пользователя.
    /// </summary>
    public class RequestOutput
    {
        /// <summary>
        /// Id заявки.
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// Id предмета заявки (заявки или бизнеса).
        /// </summary>
        public long RequestItemId { get; set; }

        /// <summary>
        /// Id пользователя, который оставил заявку.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Тип заявки (по франшизе или бизнесу).
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// Заголовок оповещения.
        /// </summary>
        public string NotifyTitle { get; set; }

        /// <summary>
        /// Описание оповещения. 
        /// </summary>
        public string NotifyDescription { get; set; }

        /// <summary>
        /// Статус заявки.
        /// </summary>
        public string RequestStatus { get; set; }
    }
}