using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "id")]
        public string PaymentId { get; set; }

        /// <summary>
        /// Ссылка на редирект после успешного холдирования.
        /// </summary>
        [JsonProperty(PropertyName = "paymentUrl")]
        public string PaymentUrl { get; set; }
    }
}
