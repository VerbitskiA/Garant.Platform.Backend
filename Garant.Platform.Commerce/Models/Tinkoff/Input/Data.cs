namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс описывает дополнительные параметры платежа.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Номер телефона покупателя.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Почта покупателя. На нее придут чеки об оплате этапа.
        /// </summary>
        public string Email { get; set; }
    }
}
