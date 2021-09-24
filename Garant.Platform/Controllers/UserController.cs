using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Mailing;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    /// <summary>
    /// Контроллер работы с пользователями сервиса.
    /// </summary>
    [ApiController, Route("user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Метод создает пользователя.
        /// </summary>
        /// <returns>Данные нового пользователя.</returns>
        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateAsync()
        {
            return Ok();
        }

        /// <summary>
        /// Метод авторизует пользователя стандартным способом.
        /// </summary>
        /// <param name="loginInput">Входная модель.</param>
        /// <returns>Данные авторизованного пользователя.</returns>
        [HttpPost, Route("login")]
        [ProducesResponseType(200, Type = typeof(ClaimOutput))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginInput loginInput)
        {
            var result = await _userService.LoginAsync(loginInput.Name, loginInput.City, loginInput.Email, loginInput.Password);

            return Ok(result);
        }

        /// <summary>
        /// Метод проверит правильность кода подтверждения.
        /// </summary>
        /// <param name="sendAcceptCodeInput">Входная модель.</param>
        /// <returns>Статус проверки.</returns>
        [HttpPost, Route("check-code")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> CheckAcceptCodeAsync([FromBody] SendAcceptCodeInput sendAcceptCodeInput)
        {
            var user = await _userService.CheckAcceptCodeAsync(sendAcceptCodeInput.Code);

            return Ok(user);
        }
    }
}
