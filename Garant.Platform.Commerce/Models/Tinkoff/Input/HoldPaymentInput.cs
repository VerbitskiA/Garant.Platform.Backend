using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс входной модели холдирования.
    /// </summary>
    public class HoldPaymentInput
    {
        [JsonProperty(PropertyName = "orderId")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public string EndDate { get; set; }

        [JsonProperty(PropertyName = "description")]
        public Description Description { get; set; }

        [JsonProperty(PropertyName = "redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty(PropertyName = "shop")]
        public Shop Shop { get; set; }
    }

    public class Shop
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
