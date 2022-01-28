using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Configurator.Services
{
    /// <summary>
    /// Класс реализует методы репозитория конфигуратора.
    /// </summary>
    public sealed class ConfiguratorRepository : IConfiguratorRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public ConfiguratorRepository(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

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
        public async Task<EmployeeEntity> CreateEmployeeAsync(string employeeRoleName, string employeeRoleSystemName, string employeeStatus, string firstName, string lastName, string patronymic, string phoneNumber, string email, string telegramTag)
        {
            try
            {
                var addEmployee = new EmployeeEntity
                {
                    Email = email,
                    EmployeeRoleName = employeeRoleName,
                    EmployeeRoleSystemName = employeeRoleSystemName,
                    EmployeeStatus = employeeStatus,
                    FirstName = firstName,
                    LastName = lastName,
                    Patronymic = patronymic,
                    PhoneNumber = phoneNumber,
                    TelegramTag = telegramTag,
                    FullName = firstName + " " + lastName + " " + (patronymic ?? string.Empty)
                };
                
                await _postgreDbContext.Employees.AddAsync(addEmployee);
                await _postgreDbContext.SaveChangesAsync();

                var result = await _postgreDbContext.Employees
                    .Where(e => e.Email.Equals(email) && e.PhoneNumber.Equals(phoneNumber))
                    .FirstOrDefaultAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <param name="email">Почта сотрудника.</param>
        /// <param name="phoneNumber">Номер телефона сотрудника.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<bool> CheckEmployeeByEmailAsync(string email, string phoneNumber)
        {
            try
            {
                var employee = await _postgreDbContext.Employees.FirstOrDefaultAsync(e => e.Email.Equals(email)) ??
                               await _postgreDbContext.Employees.FirstOrDefaultAsync(e =>
                                   e.PhoneNumber.Equals(phoneNumber));

                return employee != null;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }
    }
}