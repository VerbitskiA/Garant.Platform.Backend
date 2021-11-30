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
        public string Short { get; set; }

        /// <summary>
        /// Полное описание платежа.
        /// </summary>
        public string Full { get; set; }
    }
}
