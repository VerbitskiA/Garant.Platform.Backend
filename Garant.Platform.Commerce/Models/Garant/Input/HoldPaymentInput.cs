namespace Garant.Platform.Commerce.Models.Garant.Input
{
    /// <summary>
    /// Класс входной модели олдирования платежа.
    /// </summary>
    public class HoldPaymentInput
    {
        /// <summary>
        /// Цена.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Id бизнеса или франшизы.
        /// </summary>
        public long OriginalId { get; set; }

        /// <summary>
        /// Тип заказа.
        /// </summary>
        public string OrderType { get; set; }
    }
}
