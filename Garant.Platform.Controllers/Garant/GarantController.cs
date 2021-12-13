using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Abstraction.Garant.Vendor;
using Garant.Platform.Commerce.Models.Garant.Input;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Models.Commerce.Input;
using Garant.Platform.Models.Commerce.Output;
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
        private readonly IVendorService _vendorService;

        public GarantController(IGarantActionService _garantActionervice, ICustomerService customerService, IVendorService vendorService)
        {
            _garantActionService = _garantActionervice;
            _customerService = customerService;
            _vendorService = vendorService;
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
            var result = await _garantActionService.GetInitDataGarantAsync(paymentActionInput.OriginalId, paymentActionInput.OrderType, GetUserName(), paymentActionInput.Stage, paymentActionInput.IsChat, paymentActionInput.OtherId);

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

        /// <summary>
        /// Метод подтвердит продажу в сделке.
        /// </summary>
        /// <param name="dealInput">Входная модель.</param>
        /// <returns>Данные сделки.</returns>
        [HttpPost]
        [Route("accept-deal")]
        [ProducesResponseType(200, Type = typeof(DealOutput))]
        public async Task<IActionResult> AcceptDealAsync([FromBody] DealInput dealInput)
        {
            var result = await _vendorService.AcceptActualDealAsync(dealInput.DealItemId, dealInput.OrderType, GetUserName());

            return Ok(result);
        }
    }
}
