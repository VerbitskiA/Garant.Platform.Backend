using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Configurator.Abstractions
{
    /// <summary>
    /// Абстракция репозитория конфигуратора для работы с БД.
    /// </summary>
    public interface IConfiguratorRepository
    {
        /// <summary>
        /// Метод заведет нового сотрудника сервиса.
        /// </summary>
        /// <param name="employeeRoleName">Название роли сотрдника.</param>
        /// <param name="employeeRoleSystemName">Системное название роли сотрудника.</param>
        /// <param name="employeeStatus">Статус сотрудника.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="patronymic">Отчество.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="email">Почта сотрудника.</param>
        /// <param name="telegramTag">Тэг в телеграме.</param>
        /// <returns>Данные добавленного сотрудника</returns>
        Task<EmployeeEntity> CreateEmployeeAsync(string employeeRoleName, string employeeRoleSystemName,
            string employeeStatus, string firstName, string lastName, string patronymic, string phoneNumber,
            string email, string telegramTag);

        /// <summary>
        /// Метод проверит существование сотрудника по его почте.
        /// </summary>
        /// <param name="email">Почта сотрудника.</param>
        /// <param name="phoneNumber">Номер телефона сотрудника.</param>
        /// <returns>Статус проверки.</returns>
        Task<bool> CheckEmployeeByEmailAsync(string email, string phoneNumber);
        
        /// <summary>
        /// Метод получит список меню конфигуратора.
        /// </summary>
        /// <returns>Список меню.</returns>
        Task<IEnumerable<ConfiguratorMenuOutput>> GetMenuItemsAsync();
        
        /// <summary>
        /// Метод авторизует сотрудника сервиса.
        /// </summary>
        /// <param name="inputData">Email или телефон.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные сотрудника.</returns>
        Task<EmployeeEntity> ConfiguratorLoginAsync(string inputData, string password);
    }
}