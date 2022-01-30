using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Enums;
using Garant.Platform.Configurator.Exceptions;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.User.Output;

namespace Garant.Platform.Configurator.Services
{
    /// <summary>
    /// Класс реализует методы сервиса конфигуратора.
    /// </summary>
    public sealed class ConfiguratorService : IConfiguratorService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IConfiguratorRepository _configuratorRepository;

        public ConfiguratorService(PostgreDbContext postgreDbContext, IConfiguratorRepository configuratorRepository)
        {
            _postgreDbContext = postgreDbContext;
            _configuratorRepository = configuratorRepository;
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
        public async Task<CreateEmployeeOutput> CreateEmployeeAsync(string employeeRoleName,
            string employeeRoleSystemName, string employeeStatus, string firstName, string lastName, string patronymic,
            string phoneNumber, string email, string telegramTag)
        {
            try
            {
                // Если не все обязательные поля сотрудника заполнены.
                if (string.IsNullOrEmpty(employeeRoleName)
                    || string.IsNullOrEmpty(employeeRoleSystemName)
                    || string.IsNullOrEmpty(employeeStatus)
                    || string.IsNullOrEmpty(firstName)
                    || string.IsNullOrEmpty(lastName)
                    || string.IsNullOrEmpty(phoneNumber)
                    || string.IsNullOrEmpty(email))
                {
                    throw new EmptyEmployeeDataException();
                }

                // Проверит существование такого сотрудника.
                var checkEmployee = await _configuratorRepository.CheckEmployeeByEmailAsync(email, phoneNumber);

                // Если сотрудник уже был заведен в системе.
                if (checkEmployee)
                {
                    throw new EmployeeNotEmptyException(email, phoneNumber);
                }

                // Содрудника не найдено, заведет нового.
                var newEmployee = await _configuratorRepository.CreateEmployeeAsync(employeeRoleName,
                    employeeRoleSystemName, employeeStatus, firstName, lastName, patronymic, phoneNumber, email,
                    telegramTag);

                var mapper = AutoFac.Resolve<IMapper>();
                var result = mapper.Map<CreateEmployeeOutput>(newEmployee);

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

        /// <summary>
        /// Метод получит список меню конфигуратора.
        /// </summary>
        /// <returns>Список меню.</returns>
        public async Task<IEnumerable<ConfiguratorMenuOutput>> GetMenuItemsAsync()
        {
            try
            {
                var result = await _configuratorRepository.GetMenuItemsAsync();

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

        /// <summary>
        /// Метод авторизует сотрудника сервиса.
        /// </summary>
        /// <param name="inputData">Email или телефон.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные сотрудника.</returns>
        public async Task<ConfiguratorLoginOutput> ConfiguratorLoginAsync(string inputData, string password)
        {
            try
            {
                var loginEmployee = await _configuratorRepository.ConfiguratorLoginAsync(inputData, password);

                var mapper = AutoFac.Resolve<IMapper>();
                var result = mapper.Map<ConfiguratorLoginOutput>(loginEmployee);

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
    }
}