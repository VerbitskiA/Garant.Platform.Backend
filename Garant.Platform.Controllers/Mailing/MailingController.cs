using System.Threading.Tasks;
using Garant.Platform.Base;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Models.Mailing.Input;
using Garant.Platform.Models.Mailing.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Mailing
{
    /// <summary>
    /// Контроллер работы с рассылками.
    /// </summary>
    [ApiController, Route("mailing")]
    [Authorize]
    public class MailingController : BaseController
    {
        private readonly ICommonService _commonService;

        public MailingController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Метод отправит код подтверждения. Также запишет этот код в базу.
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost, Route("send-confirm-code")]
        //[ProducesResponseType(200, Type = typeof(MailngOutput))]
        //public async Task<IActionResult> SendMailAcceptCodeSmsAsync([FromBody] SendAcceptCodeInput sendAcceptCodeInput)
        //{
        //    var result = await _commonService.GenerateAcceptCodeAsync(sendAcceptCodeInput.Email);

        //    return Ok(result);
        //}
    }
}
