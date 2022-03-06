using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Enums;
using Garant.Platform.Configurator.Models.Output;
using Garant.Platform.Core.Consts;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.Business;
using Garant.Platform.Models.Entities.Franchise;
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

        public ConfiguratorRepository()
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
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
                    FullName = firstName + " " + lastName + " " + (patronymic ?? string.Empty),
                    Password = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
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

        /// <summary>
        /// Метод получит список меню конфигуратора.
        /// </summary>
        /// <returns>Список меню.</returns>
        public async Task<IEnumerable<ConfiguratorMenuOutput>> GetMenuItemsAsync()
        {
            try
            {
                var result = await _postgreDbContext.ConfiguratorMenuItems
                    .Select(c => new ConfiguratorMenuOutput
                    {
                        ActionName = c.ActionName,
                        MenuItemId = c.MenuItemId,
                        MenuItemName = c.MenuItemName,
                        MenuItemSysName = c.MenuItemSysName,
                        Position = c.Position
                    })
                    .ToListAsync();

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
        public async Task<EmployeeEntity> ConfiguratorLoginAsync(string inputData, string password)
        {
            try
            {
                EmployeeEntity employee = null;

                // Если почта.
                if (inputData.Contains("@"))
                {
                    var isCorrectEmail = Regex.Match(inputData, RegularExpressions.REGULAR_EMAIL);

                    // Если Email прошел проверку.
                    if (isCorrectEmail.Success)
                    {
                        // Проверит существование сотрудника.
                        employee = await _postgreDbContext.Employees.FirstOrDefaultAsync(e => e.Email.Equals(inputData));

                        // Если сотрудника не найдено.
                        if (employee == null)
                        {
                            throw new NotFoundEmployeeException();
                        }
                        
                        // Проставит доступ к панели.
                        employee.AccessPanel = (int)AccessTypePanelEnum.Granted;
                    }
                }
                
                // Если номер телефона.
                if (Regex.Match(inputData, RegularExpressions.REGULAR_PHONE_NUMBER).Success)
                {
                    // Проверит существование сотрудника.
                    employee = await _postgreDbContext.Employees.FirstOrDefaultAsync(e => e.PhoneNumber.Equals(inputData));

                    // Если сотрудника не найдено.
                    if (employee == null)
                    {
                        throw new NotFoundEmployeeException();
                    }
                    
                    // Проставит доступ к панели.
                    employee.AccessPanel = (int)AccessTypePanelEnum.Granted;
                }

                return employee;
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
                var result = await _postgreDbContext.BlogActions
                    .Select(ba => new BlogActionOutput
                    {
                        BlogActionId = ba.BlogActionId,
                        BlogActionName = ba.BlogActionName,
                        BlogActionSysName = ba.BlogActionSysName
                    })
                    .ToListAsync();

                return result;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст новую сферу.
        /// </summary>
        /// <param name="sphereName">Название сферы.</param>
        /// <param name="sphereType">Тип сферы (бизнес или франшиза).</param>
        /// <param name="sysName">Системное название сферы.</param>
        /// <returns>Созданная сфера.</returns>
        public async Task<CreateSphereOutput> CreateSphereAsync(string sphereName, string sphereType, string sysName)
        {
            try
            {
                var result = new CreateSphereOutput();
                
                // Если нужно создать сферу франшизы.
                if (sphereType.Equals("Franchise"))
                {
                    var addCategoryFran = new FranchiseCategoryEntity
                    {
                        FranchiseCategoryCode = Guid.NewGuid().ToString(),
                        FranchiseCategoryName = sphereName,
                        FranchiseCategorySysName = sysName
                    };
                    
                    await _postgreDbContext.FranchiseCategories.AddAsync(addCategoryFran);
                    await _postgreDbContext.SaveChangesAsync();

                    result = await _postgreDbContext.FranchiseCategories
                        .Where(fc => fc.FranchiseCategoryCode.Equals(addCategoryFran.FranchiseCategoryCode))
                        .Select(fc => new CreateSphereOutput
                        {
                            SphereName = addCategoryFran.FranchiseCategoryName,
                            SphereSysName = sysName,
                            SphereCode = addCategoryFran.FranchiseCategoryCode,
                            SphereType = sphereType
                        })
                        .FirstOrDefaultAsync();
                }
                
                // Если нужно создать сферу бизнеса.
                if (sphereType.Equals("Business"))
                {
                    var addCategoryBuss = new BusinessCategoryEntity()
                    {
                        BusinessCategoryCode = Guid.NewGuid().ToString(),
                        BusinessCategoryName = sphereName,
                        FranchiseCategorySysName = sysName
                    };
                    
                    await _postgreDbContext.BusinessCategories.AddAsync(addCategoryBuss);
                    await _postgreDbContext.SaveChangesAsync();

                    result = await _postgreDbContext.BusinessCategories
                        .Where(fc => fc.BusinessCategoryCode.Equals(addCategoryBuss.BusinessCategoryCode))
                        .Select(fc => new CreateSphereOutput
                        {
                            SphereName = addCategoryBuss.BusinessCategoryName,
                            SphereSysName = sysName,
                            SphereCode = addCategoryBuss.BusinessCategoryCode,
                            SphereType = sphereType
                        })
                        .FirstOrDefaultAsync();
                }
                
                return result;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст категорию сферы.
        /// </summary>
        /// <param name="sphereCode">Код сферы (guid).</param>
        /// <param name="categoryName">Название категории.</param>
        /// <param name="categoryType">Тип категории.</param>
        /// <param name="sysName">Системное название.</param>
        /// <returns>Созданная категория.</returns>
        public async Task<CreateCategoryOutput> CreateCategoryAsync(string sphereCode, string categoryName, string categoryType, string sysName)
        {
            try
            {
                // Если нужно добавить категорию сферы франшиз.
                if (categoryType.Equals("Franchise"))
                {
                    var sphereSysName = await _postgreDbContext.FranchiseCategories
                        .Where(fc => fc.FranchiseCategoryCode.Equals(sphereCode))
                        .Select(fc => fc.FranchiseCategorySysName)
                        .FirstOrDefaultAsync();
                    
                    var addCategoryFran = new FranchiseSubCategoryEntity
                    {
                        FranchiseCategoryCode = sphereCode,
                        FranchiseSubCategoryName = categoryName,
                        FranchiseSubCategorySysName = sysName,
                        FranchiseSubCategoryCode = Guid.NewGuid().ToString(),
                        FranchiseCategorySysName = sphereSysName
                    };
                    
                    await _postgreDbContext.FranchiseSubCategories.AddAsync(addCategoryFran);
                    await _postgreDbContext.SaveChangesAsync();
                }
                
                // Если нужно добавить категорию сферы бизнесов.
                if (categoryType.Equals("Business"))
                {
                    var sphereSysName = await _postgreDbContext.BusinessCategories
                        .Where(fc => fc.BusinessCategoryCode.Equals(sphereCode))
                        .Select(fc => fc.FranchiseCategorySysName)
                        .FirstOrDefaultAsync();
                    
                    var addCategoryBuss = new BusinessSubCategoryEntity
                    {
                        BusinessCategoryCode = sphereCode,
                        BusinessSubCategoryName = categoryName,
                        BusinessSubCategorySysName = sysName,
                        BusinessSubCategoryCode = Guid.NewGuid().ToString(),
                        BusinessCategorySysName = sphereSysName
                    };
                    
                    await _postgreDbContext.BusinessSubCategories.AddAsync(addCategoryBuss);
                    await _postgreDbContext.SaveChangesAsync();
                }

                var result = new CreateCategoryOutput
                {
                    SphereCode = sphereCode,
                    CategoryName = categoryName,
                    SysName = sysName,
                    CategoryType = categoryType
                };

                return result;
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