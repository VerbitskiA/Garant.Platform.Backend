using System.ComponentModel;

namespace Garant.Platform.Commerce.Models.Garant.Input
{
    /// <summary>
    /// Класс входной модели события платежа.
    /// </summary>
    public class PaymentActionInput
    {
        /// <summary>
        /// Валюта.
        /// </summary>
        [DefaultValue("RUB")]
        public string Currency { get; set; }

        /// <summary>
        /// Кол-во товара.
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Сумма (всего).
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// Список названий документов приложенных к заказу разделенные через запятую.
        /// </summary>
        public string DocumentsNames { get; set; }

        /// <summary>
        /// Тип заказа (франшиза или бизнес).
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Id исходного товара (Id франшизы или бизнеса).
        /// </summary>
        public long OriginalId { get; set; }

        /// <summary>
        /// Номер этапа.
        /// </summary>
        public int Stage { get; set; }

        /// <summary>
        /// Флаг чата.
        /// </summary>
        public bool IsChat { get; set; }

        /// <summary>
        /// Id другого пользователя.
        /// </summary>
        public string OtherId { get; set; }
    }
}
