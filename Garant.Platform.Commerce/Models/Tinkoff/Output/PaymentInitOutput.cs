using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Output
{
    /// <summary>
    /// Класс выходной модели для метода Init в системе банка. Этот метод сразу списывает средства с карты покупателя.
    /// </summary>
    public class PaymentInitOutput
    {
        /// <summary>
        /// Флаг успешного сценария при создании платежа.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Статус платежа.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Id платежа в системе банка.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Id заказа в сервисе Гарант.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Сумма в копейках.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Ссылка на платежную форму для покупателя.
        /// </summary>
        [JsonProperty(PropertyName = "PaymentURL")]
        public string PaymentUrl { get; set; }
    }
}
