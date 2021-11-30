namespace Garant.Platform.Commerce.Models.Tinkoff.Output
{
    /// <summary>
    /// Класс выходной модели при холдировании платежа.
    /// </summary>
    public class HoldPaymentOutput
    {
        /// <summary>
        /// Id платежа.
        /// </summary>
        public long PaymentId { get; set; }

        /// <summary>
        /// Ссылка на редирект после успешного холдирования.
        /// </summary>
        public string PaymentUrl { get; set; }
    }
}
