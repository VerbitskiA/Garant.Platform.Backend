namespace Garant.Platform.Models.Commerce.Input
{
    /// <summary>
    /// Класс входной модели сделки.
    /// </summary>
    public class DealInput
    {
        /// <summary>
        /// Id предмета сделки (франшизы или бизнеса).
        /// </summary>
        public long DealItemId { get; set; }

        /// <summary>
        /// Тип предмета сделки (франшиза или бизнес).
        /// </summary>
        public string OrderType { get; set; }
    }
}
