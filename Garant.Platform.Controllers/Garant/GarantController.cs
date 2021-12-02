using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Models.Garant.Input;
using Garant.Platform.Commerce.Models.Garant.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Garant
{
    /// <summary>
    /// Контроллер работы с Гарантом и сделками.
    /// </summary>
    [ApiController]
    [Route("garant")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GarantController : BaseController
    {
        private readonly IGarantActionService _garantActionService;

        public GarantController(IGarantActionService _garantActionervice)
        {
            _garantActionService = _garantActionervice;
        }

        /// <summary>
        /// Метод действия платежа.
        /// </summary>
        /// <param name="paymentActionInput">Входная модель.</param>
        /// <returns>Данные платежа.</returns>
        [HttpPost]
        [Route("payment-action")]
        [ProducesResponseType(200, Type = typeof(PaymentActionOutput))]
        public async Task<IActionResult> PaymentActionAsync([FromBody] PaymentActionInput paymentActionInput)
        {
            var result = await _garantActionService.GetTypeGarantAsync(paymentActionInput.OriginalId, paymentActionInput.OrderType, paymentActionInput.Amount, GetUserName());

            return Ok(result);
        }
    }
}
