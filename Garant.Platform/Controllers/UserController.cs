using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Mailing;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Identity;
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
        [HttpPost, Route("create")]
        //[ProducesResponseType(200, Type = typeof(UserOutput))]
        public async Task<IActionResult> CreateAsync([FromBody] UserInput userInput)
        {
            var result = await _userService.CreateAsync(userInput.Name, userInput.LastName, userInput.City, userInput.Email, userInput.Password);

            return Ok(result);
        }

        /// <summary>
        /// Метод авторизует пользователя стандартным способом.
        /// </summary>
        /// <param name="loginInput">Входная модель.</param>
        /// <returns>Данные авторизованного пользователя.</returns>
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
        [HttpPost, Route("check-code")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> CheckAcceptCodeAsync([FromBody] SendAcceptCodeInput sendAcceptCodeInput)
        {
            var user = await _userService.CheckAcceptCodeAsync(sendAcceptCodeInput.Code);

            return Ok(user);
        }
    }
}
