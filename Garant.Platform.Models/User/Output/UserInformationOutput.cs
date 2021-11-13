using System;

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
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Значения разделенные запятой.
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime DateBirth { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// ИНН.
        /// </summary>
        public int? Inn { get; set; }

        /// <summary>
        /// Расчетный счет.
        /// </summary>
        public int? Pc { get; set; }

        /// <summary>
        /// Серия паспорта.
        /// </summary>
        public int? PassportSerial { get; set; }

        /// <summary>
        /// Номер паспорта.
        /// </summary>
        public int? PassportNumber { get; set; }

        /// <summary>
        /// Дата выдачи паспорта.
        /// </summary>
        public DateTime? DateGive { get; set; }

        /// <summary>
        /// Кем выдан паспорт.
        /// </summary>
        public string WhoGive { get; set; }

        /// <summary>
        /// Код паспорта.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Адрес регистрации.
        /// </summary>
        public string AddressRegister { get; set; }

        /// <summary>
        /// Название документа.
        /// </summary>
        public string DocumentName { get; set; }
    }
}
