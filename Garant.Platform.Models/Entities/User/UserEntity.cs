using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Garant.Platform.Models.Entities.User
{
    /// <summary>
    /// Класс сопоставляется с таблицей пользователей.
    /// </summary>
    [Table("CustomUsers", Schema = "dbo")]
    public class UserEntity : IdentityUser
    {
        [Column("IsRegistrationComplete", TypeName = "boolean")]
        public bool IsRegistrationComplete { get; set; } = false;

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        [Column("UserRole", TypeName = "varchar(20)")]
        public string UserRole { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Column("LastName", TypeName = "varchar(100)")]
        public string LastName { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Column("FirstName", TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [Column("Patronymic", TypeName = "varchar(100)")]
        public string Patronymic { get; set; }

        /// <summary>
        /// Путь к иконке пользователя.
        /// </summary>
        [Column("UserIcon", TypeName = "text")]
        public string UserIcon { get; set; }

        /// <summary>
        /// Дата регистрации пользователя.
        /// </summary>
        [Column("DateRegister", TypeName = "timestamp")]
        public DateTime DateRegister { get; set; }

        [Column("City", TypeName = "varchar(200)")]
        public string City { get; set; }

        /// <summary>
        /// Код подтверждения.
        /// </summary>
        [Column("Code", TypeName = "varchar(5)")]
        public string Code { get; set; }

    }
}
