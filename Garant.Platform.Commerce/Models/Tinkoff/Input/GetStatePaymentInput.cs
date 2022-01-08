namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс входной модели статуса платежа в системе банка.
    /// </summary>
    public class GetStatePaymentInput
    {
        /// <summary>
        /// Id платежа в системе банка.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Id терминала сервиса Гарант.
        /// </summary>
        public string TerminalKey { get; set; }

        /// <summary>
        /// Токен подписи запроса.
        /// </summary>
        public string Token { get; set; }
    }
}
