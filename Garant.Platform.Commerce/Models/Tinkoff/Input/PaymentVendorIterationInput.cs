using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс входной модели для платежа за этап на счет продавца.
    /// </summary>
    public class PaymentVendorIterationInput
    {
        /// <summary>
        /// Id платежа в сервисе Гарант.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
