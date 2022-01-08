using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Output
{
    /// <summary>
    /// Класс выходной модели проверки холдированноо платежа.
    /// </summary>
    public class CheckHoldPaymentOutput
    {
        /// <summary>
        /// Статус платежа.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
