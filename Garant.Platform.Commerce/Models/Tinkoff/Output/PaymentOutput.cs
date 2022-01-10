using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Output
{
    /// <summary>
    /// Класс выходной модели платежа.
    /// </summary>
    public class PaymentOutput
    {
        /// <summary>
        /// Id платежа в сервисе Гарант.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public long PaymentId { get; set; }
    }
}
