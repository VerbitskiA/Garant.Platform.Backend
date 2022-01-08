using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс представляет поля описания платежа.
    /// </summary>
    public class Description
    {
        /// <summary>
        /// Краткое описание платежа.
        /// </summary>
        [JsonProperty(PropertyName = "short")]

        public string Short { get; set; }

        /// <summary>
        /// Полное описание платежа.
        /// </summary>
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }
}
