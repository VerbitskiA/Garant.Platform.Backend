using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.User
{
    /// <summary>
    /// Класс сопоставляется с таблицей информации о пользователе Info.UsersInformation.
    /// </summary>
    [Table("UsersInformation", Schema = "Info")]
    public class UserInformationEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long InformationId { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Column("FirstName", TypeName = "varchar(200)")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Column("LastName", TypeName = "varchar(200)")]
        public string LastName { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        [Column("City", TypeName = "varchar(100)")]
        public string City { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        [Column("Email", TypeName = "varchar(200)")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Column("Password", TypeName = "varchar(200)")]
        public string Password { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [Column("PhoneNumber", TypeName = "varchar(100)")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Значения разделенные запятой.
        /// </summary>
        [Column("Values", TypeName = "varchar(50)")]
        public string Values { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        [Column("UserId", TypeName = "varchar(50)")]
        public string UserId { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        [Column("DateBirth", TypeName = "timestamp")]
        public DateTime DateBirth { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [Column("Patronymic", TypeName = "varchar(200)")]
        public string Patronymic { get; set; }

        /// <summary>
        /// ИНН.
        /// </summary>
        [Column("Inn", TypeName = "varchar(20)")]
        public string Inn { get; set; }

        /// <summary>
        /// Расчетный счет.
        /// </summary>
        [Column("Pc", TypeName = "varchar(20)")]
        public string Pc { get; set; }

        /// <summary>
        /// КПП.
        /// </summary>
        [Column("Kpp", TypeName = "varchar(20)")]
        public string Kpp { get; set; }

        /// <summary>
        /// БИК.
        /// </summary>
        [Column("Bik", TypeName = "varchar(20)")]
        public string Bik { get; set; }

        /// <summary>
        /// Серия паспорта.
        /// </summary>
        [Column("PassportSerial", TypeName = "int")]
        public int? PassportSerial { get; set; }

        /// <summary>
        /// Номер паспорта.
        /// </summary>
        [Column("PassportNumber", TypeName = "int")]
        public int? PassportNumber { get; set; }

        /// <summary>
        /// Дата выдачи паспорта.
        /// </summary>
        [Column("DateGive", TypeName = "timestamp")]
        public DateTime? DateGive { get; set; }

        /// <summary>
        /// Кем выдан паспорт.
        /// </summary>
        [Column("WhoGive", TypeName = "varchar(400)")]
        public string WhoGive { get; set; }

        /// <summary>
        /// Код паспорта.
        /// </summary>
        [Column("Code", TypeName = "varchar(100)")]
        public string Code { get; set; }

        /// <summary>
        /// Адрес регистрации.
        /// </summary>
        [Column("AddressRegister", TypeName = "varchar(400)")]
        public string AddressRegister { get; set; }

        /// <summary>
        /// Название документа.
        /// </summary>
        [Column("DocumentName", TypeName = "varchar(400)")]
        public string DocumentName { get; set; }

        /// <summary>
        /// Название банка, которое ранее было сохранено.
        /// </summary>
        [Column("DefaultBankName", TypeName = "varchar(400)")]
        public string DefaultBankName { get; set; }

        /// <summary>
        /// Корреспондентский счёт банка получателя.
        /// </summary>
        [Column("CorrAccountNumber", TypeName = "varchar(20)")]
        public string CorrAccountNumber { get; set; }
    }
}