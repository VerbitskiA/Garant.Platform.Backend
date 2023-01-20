using Garant.Platform.Abstractions.User;
using Garant.Platform.Base;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Models.Mailing.Input;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Garant.Platform.Controllers.Auth
{
    [ApiController]
    [Route("auth")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("start-registration")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> StartRegisterAsync([FromBody] StartRegisterInput startRegister)
        {
            try
            {
                var status = await _userService.StartRegisterUserAsync(startRegister.Email);

                if (status)
                {
                    return Ok(new { status = true, message = $"Код подтверждения выслан на почту {startRegister.Email}." });
                }
                else
                {
                    return StatusCode(500, new { status = false, message = "Что-то пошло не так :(" } );
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

        [AllowAnonymous]
        [HttpPost]
        [Route("resend-code")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ResendCodeAsync([FromBody] StartRegisterInput resendingCode)
        {
            try
            {
                var status = await _userService.ResendCodeAsync(resendingCode.Email);

                if (status)
                {
                    return Ok(new { status = true, message = $"Код подтверждения выслан заново на почту {resendingCode.Email}." });
                }
                else
                {
                    return StatusCode(500, new { status = false, message = "Что-то пошло не так :(" });
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

        [AllowAnonymous]
        [HttpPost] 
        [Route("check-code")]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200, Type = typeof(ClaimOutput))]
        public async Task<IActionResult> LoginAsync([FromBody] UserInput loginInput)
        {
            try
            {
                var result = await _userService.AuthenticateUserAsync(loginInput.Email, loginInput.Password);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
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
    }
}
