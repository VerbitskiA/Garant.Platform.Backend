using System;

namespace Garant.Platform.Models.User.Input
{
    /// <summary>
    /// Класс входной модели для добавления доп. информации пользователя.
    /// </summary>
    public class UserInformationInput
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
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Название причин регистрации разделенные запятыми.
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// Тип формы, данные которой нужно сохранить.
        /// </summary>
        public string TypeForm { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime DateBirth { get; set; }
    }
}
