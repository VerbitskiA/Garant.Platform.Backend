using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Exceptions;
using Garant.Platform.Configurator.Models.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.Franchise;
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
        private readonly IFranchiseService _franchiseService;
        private readonly IBusinessService _businessService;
        private readonly IFranchiseRepository _franchiseRepository;
        private readonly IBusinessRepository _businessRepository;

        public ConfiguratorService(PostgreDbContext postgreDbContext, 
            IConfiguratorRepository configuratorRepository,
            IFranchiseService franchiseService,
            IBusinessService businessService,
            IFranchiseRepository franchiseRepository,
            IBusinessRepository businessRepository)
        {
            _postgreDbContext = postgreDbContext;
            _configuratorRepository = configuratorRepository;
            _franchiseService = franchiseService;
            _businessService = businessService;
            _franchiseRepository = franchiseRepository;
            _businessRepository = businessRepository;

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

        /// <summary>
        /// Метод получит список действий при работе с блогами.
        /// </summary>
        /// <returns>Список действий.</returns>
        public async Task<IEnumerable<BlogActionOutput>> GetBlogActionsAsync()
        {
            try
            {
                var result = await _configuratorRepository.GetBlogActionsAsync();

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
        /// Метод утвердит карточку. После этого карточка попадает в каталоги.
        /// </summary>
        /// <param name="cardId">Id карточки.</param>
        /// <param name="cardType">Тип карточки.</param>
        /// <returns>Статус утверждения.</returns>
        public async Task<bool> AcceptCardAsync(long cardId, string cardType)
        {
            try
            {
                if (cardId <= 0)
                {
                    throw new EmptyCardIdException(cardId);
                }

                if (string.IsNullOrEmpty(cardType))
                {
                    throw new EmptyCardTypeException(cardType);
                }

                if (cardType.Equals("Franchise"))
                {
                    var franchise = await _franchiseService.GetFranchiseAsync(cardId);

                    if (franchise != null)
                    {
                        //TODO тут ошибка обновления а вообще это вынести в репозиторий франшиз
                        franchise.IsAccepted = true;
                        _postgreDbContext.Franchises.Update(franchise);
                        await _postgreDbContext.SaveChangesAsync();

                        return true;
                    }
                    
                    return false;
                }
                
                if (cardType.Equals("Business"))
                {
                    var business = await _businessService.GetBusinessAsync(cardId);
                    
                    if (business != null)
                    {
                        business.IsAccepted = true;
                        await _postgreDbContext.SaveChangesAsync();

                        return true;
                    }
                    
                    return false;
                }

                return false;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}