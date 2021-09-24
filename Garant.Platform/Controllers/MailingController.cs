using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    /// <summary>
    /// Контроллер работы с рассылками.
    /// </summary>
    [ApiController, Route("mailing")]
    public class MailingController : BaseController
    {
        private readonly ICommonService _commonService;

        public MailingController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Метод отправит код подтверждения по смс. Также запишет этот код в базу.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("send-sms-confirm-code")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> SendMailAcceptCodeSmsAsync([FromQuery] string number)
        {
            await _commonService.GenerateAcceptCodeAsync(number, "sms");

            return Ok();
        }
    }
}
