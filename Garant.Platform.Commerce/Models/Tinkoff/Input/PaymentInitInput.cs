using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс входной модели для метода Init в системе банка. Этот метод сразу списывает средства с карты покупателя.
    /// </summary>
    public class PaymentInitInput
    {
        /// <summary>
        /// Ключ терминала сервиса Гарант.
        /// </summary>
        public string TerminalKey { get; set; }

        /// <summary>
        /// Сумма платежа после вычета комиссии.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Id заказа в сервисе Гарант.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Описание заказа.
        /// </summary>
        public string Description { get; set; }

        [JsonProperty(PropertyName = "DATA")]
        public Data Data { get; set; }

        /// <summary>
        /// Массив данных чека.
        /// </summary>
        public Receipt Receipt { get; set; }

        /// <summary>
        /// Тип оплаты.
        /// </summary>
        public string PayType { get; set; }
    }
}
