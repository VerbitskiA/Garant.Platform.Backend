namespace Garant.Platform.Models.Request.Output
{
    /// <summary>
    /// Класс выходной модели сделки.
    /// </summary>
    public class RequestDealOutput
    {
        public long ItemId { get; set; }

        /// <summary>
        /// Id бизнеса.
        /// </summary>
        public long ItemDealIdId { get; set; }

        /// <summary>
        /// ФИО текущего пользователя, который оставил заявку.
        /// </summary>
        public string CurrentUserName { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Заголовок предмета сделки.
        /// </summary>
        public string DealItemTitle { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ФИО владельца предмета сделки.
        /// </summary>
        public string OwnerDeaItemUserName { get; set; }

        /// <summary>
        /// Центральный текст "Сделка между".
        /// </summary>
        public string MiddleText { get; set; }

        /// <summary>
        /// Иконка профиля текущего пользователя.
        /// </summary>
        public string CurrentUserIconUrl { get; set; }

        /// <summary>
        /// Иконка профиля владельца предмета сделки.
        /// </summary>
        public string OwnerUserIconUrl { get; set; }

        /// <summary>
        /// Текст кнопки действия.
        /// </summary>
        public string ButtonActionText { get; set; }
    }
}