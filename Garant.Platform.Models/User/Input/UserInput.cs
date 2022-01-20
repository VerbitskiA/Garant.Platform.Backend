namespace Garant.Platform.Models.User.Input
{
    /// <summary>
    /// Класс входной модели пользователя.
    /// </summary>
    public class UserInput
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public string Role { get; set; }
    }
}
