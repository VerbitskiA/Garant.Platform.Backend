using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Input;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Mailing.Input;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    /// <summary>
    /// Контроллер работы с пользователями сервиса.
    /// </summary>
    [ApiController, Route("user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;

        public UserController(IUserService userService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Метод создает пользователя.
        /// </summary>
        /// <returns>Данные нового пользователя.</returns>
        [AllowAnonymous]
        [HttpPost, Route("create")]
        [ProducesResponseType(200, Type = typeof(UserOutput))]
        public async Task<IActionResult> CreateAsync([FromBody] UserInput userInput)
        {
            var result = await _userService.CreateAsync(userInput.Name, userInput.LastName, userInput.City, userInput.Email, userInput.Password, userInput.Role);

            return Ok(result);
        }

        /// <summary>
        /// Метод авторизует пользователя стандартным способом.
        /// </summary>
        /// <param name="loginInput">Входная модель.</param>
        /// <returns>Данные авторизованного пользователя.</returns>
        [AllowAnonymous]
        [HttpPost, Route("login")]
        [ProducesResponseType(200, Type = typeof(ClaimOutput))]
        public async Task<IActionResult> LoginAsync([FromBody] UserInput loginInput)
        {
            var result = await _userService.LoginAsync(loginInput.Email, loginInput.Password);

            return Ok(result);
        }

        /// <summary>
        /// Метод проверит правильность кода подтверждения.
        /// </summary>
        /// <param name="sendAcceptCodeInput">Входная модель.</param>
        /// <returns>Статус проверки.</returns>
        [AllowAnonymous]
        [HttpPost, Route("check-code")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> CheckAcceptCodeAsync([FromBody] SendAcceptCodeInput sendAcceptCodeInput)
        {
            var user = await _userService.CheckAcceptCodeAsync(sendAcceptCodeInput.Code);

            return Ok(user);
        }

        /// <summary>
        /// Метод получит список полей основного хидера.
        /// </summary>
        /// <param name="headerInput">Входная модель.</param>
        /// <returns>Список полей хидера.</returns>
        [AllowAnonymous]
        [HttpPost, Route("init-header")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<HeaderOutput>))]
        public async Task<IActionResult> InitHeaderAsync([FromBody] HeaderInput headerInput)
        {
            var result = await _userService.InitHeaderAsync(headerInput.Type);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список полей футера.
        /// </summary>
        /// <returns>Список полей футера.</returns>
        [AllowAnonymous]
        [HttpPost, Route("init-footer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FooterOutput>))]
        public async Task<IActionResult> InitFooterAsync()
        {
            var result = await _userService.InitFooterAsync();

            return Ok(result);
        }
    }
}
