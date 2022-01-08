namespace Garant.Platform.Commerce.Models.Tinkoff.Output
{
    /// <summary>
    /// Класс выходной модели для проверки статуса платежа.
    /// </summary>
    public class GetPaymentStatusOutput
    {
        /// <summary>
        /// Флаг успешного платежа.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Статус платежа.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Id заказа в системе банка.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Id заказа в сервисе Гарант.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Цена в копейках.
        /// </summary>
        public string Amount { get; set; }
    }
}
