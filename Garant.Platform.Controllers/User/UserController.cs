using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Input;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Mailing.Input;
using Garant.Platform.Models.Suggestion.Input;
using Garant.Platform.Models.Suggestion.Output;
using Garant.Platform.Models.Transition.Input;
using Garant.Platform.Models.Transition.Output;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.User
{
    /// <summary>
    /// Контроллер работы с пользователями сервиса.
    /// </summary>
    [ApiController, Route("user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [AllowAnonymous]
        [HttpPost, Route("start-register")]
        [ProducesResponseType(200, Type = typeof(ClaimOutput))]
        public async Task<IActionResult> StartRegisterAsync([FromBody] StartRegisterInput startRegister)
        {
            try
            {
                var status = await _userService.StartRegisterUserAsync(startRegister.Email);

                if (status)
                {
                    return Ok(new { message = $"Код подтверждения выслан на почту {startRegister.Email}." });
                }
                else
                {
                    return StatusCode(500, "Что-то пошло не так :(");
                }
            }
            catch (RegisterFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Метод создает пользователя.
        /// </summary>
        /// <returns>Данные нового пользователя.</returns>
        [AllowAnonymous]
        [HttpPost, Route("resend-code")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ResendCodeAsync([FromBody] StartRegisterInput resendingCode)
        {
            try
            {
                var status = await _userService.ResendCodeAsync(resendingCode.Email);

                if (status)
                {
                    return Ok(new { message = $"Код подтверждения выслан заново на почту {resendingCode.Email}." });
                }
                else
                {
                    return StatusCode(500, "Что-то пошло не так :(");
                }
            }
            catch (RegisterFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
            try
            {
                var status = await _userService.CheckAcceptCodeAsync(sendAcceptCodeInput.Email, sendAcceptCodeInput.Code);

                if (status)
                {
                    return Ok(new { status = status, message = $"Код успешно подтвеждён." });
                }
                else
                {
                    return Ok(new { status = status, message = $"Ошибка! Указан неверный код!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Метод проверит правильность кода подтверждения.
        /// </summary>
        /// <param name="sendAcceptCodeInput">Входная модель.</param>
        /// <returns>Статус проверки.</returns>
        [AllowAnonymous]
        [HttpPost, Route("complete-registration")]
        [ProducesResponseType(200, Type = typeof(ClaimOutput))]
        public async Task<IActionResult> CompleteRegistationAsync([FromBody] CompleteRegistrationInput completeRegistration)
        {
            try
            {
                var user = await _userService.CompleteRegisterAsync(completeRegistration);

                return Ok(user);
            }
            catch (RegisterFailedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
            try
            {
                var result = await _userService.AuthenticateUserAsync(loginInput.Email, loginInput.Password);

                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new {status = false, message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost, Route("refresh-token")]
        [ProducesResponseType(200, Type = typeof(ClaimOutput))]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken)
        {
            try
            {
                var result = await _userService.RefreshAccessTokenAsync(refreshToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpGet, Route("test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                return Ok("TEST TEST TEST");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #region header
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

        /// <summary>
        /// Метод завершит регистрацию и добавит информацию о пользователе.
        /// </summary>
        /// <param name="userInformationInput">Входная модель.</param>
        /// <returns>Данные пользователя.</returns>
        [AllowAnonymous]
        [HttpPost, Route("save-user-info")]
        [ProducesResponseType(200, Type = typeof(UserInformationOutput))]
        public async Task<IActionResult> SaveUserInfoAsync([FromBody] UserInformationInput userInformationInput)
        {
            var result = await _userService.SaveUserInfoAsync(userInformationInput.FirstName, userInformationInput.LastName, userInformationInput.City, userInformationInput.Email, userInformationInput.Password, userInformationInput.Values, userInformationInput.Kpp, userInformationInput.Bik, userInformationInput.DefaultBankName, GetUserName(), userInformationInput.CorrAccountNumber, userInformationInput.Inn);

            return Ok(result);
        }

        /// <summary>
        /// Метод подтвердит почту по временному коду (guid).
        /// </summary>
        /// <param name="confirmEmailInput">Входная модель.</param>
        /// <returns>Флаг подтверждения.</returns>
        [AllowAnonymous]
        [HttpPost, Route("confirm-email")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailInput confirmEmailInput)
        {
            var result = await _userService.ConfirmEmailAsync(confirmEmailInput.Code);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит одно предложение с флагом IsSingle.
        /// </summary>
        /// <param name="suggestionInput">Входная модель.</param>
        /// <returns>Данные предложения.</returns>
        [AllowAnonymous]
        [HttpPost, Route("single-suggestion")]
        [ProducesResponseType(200, Type = typeof(SuggestionOutput))]
        public async Task<IActionResult> GetSingleSuggestionAsync([FromBody] SuggestionInput suggestionInput)
        {
            var result = await _userService.GetSingleSuggestion(suggestionInput.IsSingle, suggestionInput.IsAll);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит все предложения с флагом IsAll.
        /// </summary>
        /// <param name="suggestionInput">Входная модель.</param>
        /// <returns>Список предложений.</returns>
        [HttpPost, Route("all-suggestions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SuggestionOutput>))]
        public async Task<IActionResult> GetAllSuggestionsAsync([FromBody] SuggestionInput suggestionInput)
        {
            var result = await _userService.GetAllSuggestionsAsync(suggestionInput.IsSingle, suggestionInput.IsAll);

            return Ok(result);
        }

        ///// <summary>
        ///// Метод обновит токен.
        ///// </summary>
        ///// <returns>Новый токен.</returns>
        //[AllowAnonymous]
        //[HttpGet, Route("token")]
        //[ProducesResponseType(200, Type = typeof(ClaimOutput))]
        //public async Task<IActionResult> GenerateTokenAsync()
        //{
        //    var result = await _userService.GenerateTokenAsync();

        //    return Ok(result);
        //}

        /// <summary>
        /// Метод сформирует хлебные крошки для страницы.
        /// </summary>
        /// <param name="breadcrumbInput">Входная модель.</param>
        /// <returns>Список хлебных крошек.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("get-breadcrumbs")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BreadcrumbOutput>))]
        public async Task<IActionResult> GetBreadcrumbsAsync([FromBody] BreadcrumbInput breadcrumbInput)
        {
            var result = await _userService.GetBreadcrumbsAsync(breadcrumbInput.SelectorPage);

            return Ok(result);
        }

        /// <summary>
        /// Метод запишет переход пользователя.
        /// </summary>
        /// <param name="transitionInput">Входная модель.</param>
        /// <returns>Флаг записи перехода.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("set-transition")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> SetTransitionAsync([FromBody] TransitionInput transitionInput)
        {
            var result = await _userService.SetTransitionAsync(GetUserName(), transitionInput.TransitionType, transitionInput.ReferenceId, transitionInput.OtherId, transitionInput.TypeItem);

            return Ok(result);
        }

        /// <summary>
        /// Метод получит переход пользователя.
        /// </summary>
        /// <returns>Данные перехода.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("get-transition")]
        [ProducesResponseType(200, Type = typeof(TransitionOutput))]
        public async Task<IActionResult> GetTransitionAsync()
        {
            var result = await _userService.GetTransitionAsync(GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит переход пользователя по параметрам.
        /// </summary>
        /// <returns>Данные перехода.</returns>
        [HttpPost]
        [Route("get-transition-with-params")]
        [ProducesResponseType(200, Type = typeof(TransitionOutput))]
        public async Task<IActionResult> getTransitionWithParamsAsync([FromBody] TransitionInput transitionInput)
        {
            var result = await _userService.GetTransitionWithParamsAsync(transitionInput.ReferenceId, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит фио авторизованного пользователя.
        /// </summary>
        /// <returns>Данные пользователя.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("user-fio")]
        [ProducesResponseType(200, Type = typeof(UserOutput))]
        public async Task<IActionResult> GetUserFioAsync()
        {
            var result = await _userService.GetUserFioAsync(GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод проверит, заполнил ил пользователь данные о себе. 
        /// </summary>
        /// <returns>Флаг проверки.</returns>
        [HttpPost]
        [Route("check-profile-data")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> IsWriteProfileDataAsync()
        {
            var result = await _userService.IsWriteProfileDataAsync(GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод сохранит данные формы профиля пользователя.
        /// </summary>
        /// <param name="userInformationInput">Входная модель.</param>
        /// <returns>Данные формы.</returns>
        [HttpPost]
        [Route("save-profile-info")]
        [ProducesResponseType(200, Type = typeof(UserInformationOutput))]
        public async Task<IActionResult> SaveProfileFormAsync([FromForm] IFormCollection documentFile, [FromForm] string userInformationInput)
        {
            var result = await _userService.SaveProfileFormAsync(documentFile, userInformationInput, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит информацию профиля пользователя.
        /// </summary>
        /// <returns>Данные профиля.</returns>
        [HttpPost]
        [Route("get-profile-info")]
        [ProducesResponseType(200, Type = typeof(UserInformationOutput))]
        public async Task<IActionResult> GetProfileInfoAsync()
        {
            var result = await _userService.GetProfileInfoAsync(GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод получит список меню для пунктов ЛК.
        /// </summary>
        /// <returns>Список пунктов ЛК.</returns>
        [HttpPost]
        [Route("profile-menu")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProfileNavigationOutput>))]
        public async Task<IActionResult> GetProfileMenuListAsync()
        {
            var result = await _userService.GetProfileMenuListAsync();

            return Ok(result);
        }
        #endregion
    }
}
