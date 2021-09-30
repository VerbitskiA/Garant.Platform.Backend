namespace Garant.Platform.Models.User.Output
{
    /// <summary>
    /// Класс выходной модели для добавления доп. информации пользователя.
    /// </summary>
    public class UserInformationOutput
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Название причины регистрации.
        /// </summary>
        public string VariantForWhatName { get; set; }

        /// <summary>
        /// Код причины регистрации.
        /// </summary>
        public string VariantForWhatCode { get; set; }
    }
}
