using System;

namespace Garant.Platform.Models.User.Output
{
    /// <summary>
    /// Класс входной модели пользователя.
    /// </summary>
    public class UserOutput
    {
        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Дата регистрации.
        /// </summary>
        public DateTime DateRegister { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }
    }
}
