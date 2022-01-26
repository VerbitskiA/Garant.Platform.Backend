using Garant.Platform.Base;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Models.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Configurator.Controllers
{
    /// <summary>
    /// Контроллер для работы с конфигуратором.
    /// </summary>
    [ApiController, Route("configurator")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ConfiguratorController : BaseController
    {
        private readonly IConfiguratorService _configuratorService;
        
        public ConfiguratorController(IConfiguratorService configuratorService)
        {
            _configuratorService = configuratorService;
        }

        /// <summary>
        /// Метод заведет нового сотрудника сервиса.
        /// </summary>
        /// <param name="createEmployeeInput">Входная модель.</param>
        /// <returns>Данные сотрудника.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("employee")]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeInput createEmployeeInput)
        {
            var result = await _configuratorService.CreateEmployeeAsync(createEmployeeInput.EmployeeRoleName, createEmployeeInput.EmployeeRoleSystemName, createEmployeeInput.EmployeeStatus, createEmployeeInput.FirstName, createEmployeeInput.LastName, createEmployeeInput.Patronymic, createEmployeeInput.PhoneNumber, createEmployeeInput.Email, createEmployeeInput.TelegramTag);
            
            return Ok(result);
        }
    }
}