namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс входной модели для получения статусов всех платежей итерации этапа.
    /// </summary>
    public class StatePaymentInput
    {
        /// <summary>
        /// Id платежа в системе банка.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Id заказа.
        /// </summary>
        public long OrderId { get; set; }
    }
}
