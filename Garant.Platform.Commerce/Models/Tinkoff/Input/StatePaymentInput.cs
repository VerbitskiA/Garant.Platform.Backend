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

        /// <summary>
        /// Тип предмета обсуждения.
        /// </summary>
        public string DealItemType { get; set; }

        /// <summary>
        /// Id предмета обсуждения.
        /// </summary>
        public long ItemDealId { get; set; }
    }
}
