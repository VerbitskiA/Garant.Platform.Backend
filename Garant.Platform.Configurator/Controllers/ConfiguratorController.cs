using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Base;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Models.Input;
using Garant.Platform.Models.Configurator.Input;
using Garant.Platform.Models.Configurator.Output;
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

        /// <summary>
        /// TODO: когда будет доработан контроль токена для внутренних сотрудников сервиса, то убрать AllowAnonymous.
        /// Метод получит список меню конфигуратора.
        /// </summary>
        /// <returns>Список меню.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("menu-items")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ConfiguratorMenuOutput>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMenuItemsAsync()
        {
            var result = await _configuratorService.GetMenuItemsAsync();
            
            return Ok(result);
        }

        /// <summary>
        /// Метод авторизует сотрудника сервиса.
        /// </summary>
        /// <param name="configuratorLoginInput">Входная модель.</param>
        /// <returns>Данные сотрудника.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200, Type = typeof(ConfiguratorLoginOutput))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ConfiguratorLoginAsync([FromBody] ConfiguratorLoginInput configuratorLoginInput)
        {
            var result = await _configuratorService.ConfiguratorLoginAsync(configuratorLoginInput.InputData, configuratorLoginInput.Password);
            
            return Ok(result);
        }
    }
}