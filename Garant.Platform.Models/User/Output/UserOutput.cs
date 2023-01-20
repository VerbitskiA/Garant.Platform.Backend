﻿using System;

namespace Garant.Platform.Models.User.Output
{
    /// <summary>
    /// Класс входной модели пользователя.
    /// </summary>
    public class UserOutput
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Дата регистрации.
        /// </summary>
        public DateTime DateRegister { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; set; }

        public string City { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Флаг наличия ответов на вопросы.
        /// </summary>
        public bool IsWriteQuestion { get; set; }

        /// <summary>
        /// Полные ФИО.
        /// </summary>
        public string FullName { get; set; }

        public string Code { get; set; }

        public string UserRole { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
