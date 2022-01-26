using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garant.Platform.Configurator.Models.Input
{
    /// <summary>
    /// Класс входной модели для создания нового сотрудника сервиса.
    /// </summary>
    public class CreateEmployeeInput
    {
        /// <summary>
        /// Роль сотрудника на проекте.
        /// </summary>
        [Required]
        public string EmployeeRoleName { get; set; }

        /// <summary>
        /// Системное имя роли сотрудника.
        /// </summary>
        [Required]
        public string EmployeeRoleSystemName { get; set; }

        /// <summary>
        /// Статус сотрудника на проекте.
        /// </summary>
        [DefaultValue("Не назначено")]
        public string EmployeeStatus { get; set; }

        /// <summary>
        /// Имеет ли доступ к панели.
        /// 1 - да.
        /// 0 - нет.
        /// </summary>
        [DefaultValue(0)]
        public int AccessPanel { get; set; }

        /// <summary>
        /// Фамилия сотрудника.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Полное ФИО.
        /// </summary>
        public string FullName => FirstName + " " + LastName + " " + (Patronymic ?? string.Empty);

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Тег в телеграм.
        /// </summary>
        public string TelegramTag { get; set; }
    }
}