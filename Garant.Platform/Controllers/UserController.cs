using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.User.Input;
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
        public async Task<IActionResult> LoginAsync([FromBody] LoginInput loginInput)
        {
            return Ok();
        }
    }
}
