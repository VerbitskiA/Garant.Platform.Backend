using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Base;
using Garant.Platform.Configurator.Abstractions;
using Garant.Platform.Configurator.Models.Input;
using Garant.Platform.Configurator.Models.Output;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Configurator.Input;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Franchise.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IFranchiseService _franchiseService;
        private readonly IBusinessService _businessService;
        
        public ConfiguratorController(IConfiguratorService configuratorService, IFranchiseService franchiseService, IBusinessService businessService)
        {
            _configuratorService = configuratorService;
            _franchiseService = franchiseService;
            _businessService = businessService;
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

        /// <summary>
        /// TODO: когда будет доработан контроль токена для внутренних сотрудников сервиса, то убрать AllowAnonymous.
        /// Метод получит список действий при работе с блогами.
        /// </summary>
        /// <returns>Список действий.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("blog-actions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BlogActionOutput>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403, Type = typeof(string))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBlogActionsAsync()
        {
            var result = await _configuratorService.GetBlogActionsAsync();
            
            return Ok(result);
        }
        
        /// <summary>
        /// Метод создаст новую или обновит существующую франшизу.
        /// </summary>
        /// <param name="franchiseFilesInput">Входные файлы.</param>
        /// <param name="franchiseDataInput">Данные в строке json.</param>
        /// <returns>Данные франшизы.</returns>
        [AllowAnonymous]
        [HttpPost, Route("create-update-franchise")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateFranchiseOutput))]
        public async Task<IActionResult> CreateUpdateFranchiseConfiguratorAsync([FromForm] IFormCollection franchiseFilesInput, [FromForm] string franchiseDataInput)
        {
            var result = await _franchiseService.CreateUpdateFranchiseAsync(franchiseFilesInput, franchiseDataInput, GetUserName() ?? "info@gobizy.com");

            return Ok(result);
        }
        
        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        [AllowAnonymous]
        [HttpPost]
        [Route("temp-file")]
        public async Task<IActionResult> AddTempFilesBeforeCreateFranchiseAsync([FromForm] IFormCollection files)
        {
            var result = await _franchiseService.AddTempFilesBeforeCreateFranchiseAsync(files, GetUserName());

            return Ok(result);
        }
        
        /// <summary>
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("get-franchise")]
        [ProducesResponseType(200, Type = typeof(FranchiseOutput))]
        public async Task<FranchiseOutput> GetFranchiseAsync([FromQuery] long franchiseId)
        {
            var result = await _franchiseService.GetFranchiseAsync(franchiseId);

            return result;
        }
        
        /// <summary>
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("get-business")]
        [ProducesResponseType(200, Type = typeof(BusinessOutput))]
        public async Task<BusinessOutput> GetBusinessAsync([FromQuery] long businessId)
        {
            var result = await _businessService.GetBusinessAsync(businessId);

            return result;
        }
        
        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("create-update-business")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateBusinessOutput))]
        public async Task<IActionResult> CreateUpdateBusinessAsync([FromForm] IFormCollection businessFilesInput, [FromForm] string businessDataInput)
        {
            var result = await _businessService.CreateUpdateBusinessAsync(businessFilesInput, businessDataInput, GetUserName() ?? "info@gobizy.com");

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список франшиз, которые ожидают согласования.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("franchises-not-accepted")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FranchiseOutput>))]
        public async Task<IEnumerable<FranchiseOutput>> GetNotAcceptedFranchisesAsync()
        {
            var result = await _franchiseService.GetNotAcceptedFranchisesAsync();

            return result;
        }

        /// <summary>
        /// Метод утвердит карточку. После этого карточка попадает в каталоги.
        /// </summary>
        /// <param name="cardId">Id карточки.</param>
        /// <param name="cardType">Тип карточки.</param>
        /// <returns>Статус утверждения.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("accept-card")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> AcceptCardAsync([FromQuery] [Required] long cardId, [Required] string cardType)
        {
            var result = await _configuratorService.AcceptCardAsync(cardId, cardType);

            return result;
        }

        /// <summary>
        /// Метод отклонит публикацию карточки. 
        /// </summary>
        /// <param name="cardId">Id карточки.</param>
        /// <param name="cardType">Тип карточки.</param>
        /// <param name="comment">Комментарий отклонения.</param>
        /// <returns>Статус отклонения.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("reject-card")]
        public async Task<bool> RejectCardAsync([FromQuery] [Required] long cardId, [Required] string cardType, [Required] string comment)
        {
            var result = await _configuratorService.RejectCardAsync(cardId, cardType, comment);

            return result;
        }
    }
}