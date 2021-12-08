using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
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
        private readonly ICustomerService _customerService;

        public GarantController(IGarantActionService _garantActionervice, ICustomerService customerService)
        {
            _garantActionService = _garantActionervice;
            _customerService = customerService;
        }

        /// <summary>
        /// Метод получит данные для стартовой страницы в Гаранте..
        /// </summary>
        /// <param name="paymentActionInput">Входная модель.</param>
        /// <returns>Данные стартовой страницы.</returns>
        [HttpPost]
        [Route("init")]
        [ProducesResponseType(200, Type = typeof(PaymentActionOutput))]
        public async Task<IActionResult> PaymentActionAsync([FromBody] PaymentActionInput paymentActionInput)
        {
            var result = await _garantActionService.GetInitDataGarantAsync(paymentActionInput.OriginalId, paymentActionInput.OrderType, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод холдирует платеж.
        /// </summary>
        /// <param name="holdPaymentInput">Входная модель.</param>
        /// <returns>Данные платежа.</returns>
        [HttpPost]
        [Route("hold-payment")]
        public async Task<IActionResult> HoldPaymentAsync([FromBody] HoldPaymentInput holdPaymentInput)
        {
            var result = await _customerService.HoldPaymentAsync(holdPaymentInput.OriginalId, holdPaymentInput.Amount, GetUserName(), holdPaymentInput.OrderType);

            return Ok(result);
        }
    }
}
