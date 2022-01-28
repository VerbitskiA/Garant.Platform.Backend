using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.User
{
    /// <summary>
    /// Класс сопоставляется с таблицей сотрудников сервиса Гарант dbo.Employees.
    /// </summary>
    [Table("Employees", Schema = "dbo")]
    public class EmployeeEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key] 
        public long EmployeeId { get; set; }

        /// <summary>
        /// Роль сотрудника на проекте.
        /// </summary>
        [Required]
        [Column("EmployeeRoleName", TypeName = "varchar(200)")]
        public string EmployeeRoleName { get; set; }

        /// <summary>
        /// Системное имя роли сотрудника.
        /// </summary>
        [Required]
        [Column("EmployeeRoleSystemName", TypeName = "varchar(300)")]
        public string EmployeeRoleSystemName { get; set; }

        /// <summary>
        /// Статус сотрудника на проекте.
        /// </summary>
        [Column("EmployeeStatus", TypeName = "varchar(100)")]
        [DefaultValue("Не назначено")]
        public string EmployeeStatus { get; set; }

        /// <summary>
        /// Имеет ли доступ к панели.
        /// 1 - да.
        /// 0 - нет.
        /// </summary>
        [DefaultValue(0)]
        [Column("AccessPanel", TypeName = "int")]
        public int AccessPanel { get; set; }

        /// <summary>
        /// Фамилия сотрудника.
        /// </summary>
        [Required]
        [Column("FirstName", TypeName = "varchar(150)")]
        public string FirstName { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Required]
        [Column("LastName", TypeName = "varchar(150)")]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [Column("Patronymic", TypeName = "varchar(150)")]
        public string Patronymic { get; set; }

        /// <summary>
        /// Полное ФИО.
        /// </summary>
        [Column("FullName", TypeName = "varchar(200)")]
        public string FullName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [Required]
        [Column("PhoneNumber", TypeName = "varchar(150)")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        [Required]
        [Column("Email", TypeName = "varchar(200)")]
        public string Email { get; set; }

        /// <summary>
        /// Тег в телеграм.
        /// </summary>
        [Column("TelegramTag", TypeName = "varchar(150)")]
        public string TelegramTag { get; set; }
    }
}