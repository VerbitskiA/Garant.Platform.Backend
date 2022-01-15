using System.ComponentModel.DataAnnotations;
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
        [Required]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Реквизиты плательщика.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "from")]
        public From From { get; set; }

        /// <summary>
        /// Реквизиты получателя.
        /// </summary>
        [JsonProperty(PropertyName = "to")]
        public To To { get; set; }

        [JsonProperty(PropertyName = "purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// Сумма в руб.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }

        /// <summary>
        /// Удержанная сумма в руб.
        /// </summary>
        [JsonProperty(PropertyName = "collectionAmount")]
        public double? CollectionAmount { get; set; }
    }

    /// <summary>
    /// Класс описывает структуру реквизитов плательщика.
    /// </summary>
    public class From
    {
        [Required]
        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }
    }

    /// <summary>
    /// Класс описывает структуру реквизитов получателя платежа.
    /// </summary>
    public class To
    {
        /// <summary>
        /// Получатель.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// ИНН получателя.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "inn")]
        public string Inn { get; set; }

        /// <summary>
        /// КПП получателя.
        /// </summary>
        [JsonProperty(PropertyName = "kpp")]
        public string Kpp { get; set; }

        /// <summary>
        /// БИК банка получателя.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "bik")]
        public string Bik { get; set; }

        /// <summary>
        /// Наименование банка получателя. 
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "bankName")]
        public string BankName { get; set; }

        /// <summary>
        /// Корреспондентский счёт банка получателя.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "corrAccountNumber")]
        public string CorrAccountNumber { get; set; }

        /// <summary>
        /// Корреспондентский счёт банка получателя.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }
    }
}
