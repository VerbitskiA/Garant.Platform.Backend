using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс входной модели платежа.
    /// </summary>
    public class PaymentInput
    {
        /// <summary>
        /// Id магазина сервиса Гарант.
        /// </summary>
        [JsonProperty(PropertyName = "shopId")]
        public string ShopId { get; set; }

        /// <summary>
        /// Сумма платежа после вычисления комиссии.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }
    }
}
