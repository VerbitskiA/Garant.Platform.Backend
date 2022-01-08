namespace Garant.Platform.Commerce.Models.Garant.Output
{
    /// <summary>
    /// Класс выходной модели с данными пользователя создавшего заказ.
    /// </summary>
    public class FindUserPhoneEmailCreatedOrderOutput
    {
        /// <summary>
        /// Почта пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Телефон пользователя.
        /// </summary>
        public string Phone { get; set; }
    }
}
