using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Garant;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Abstraction.Garant.Vendor;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Models.Garant.Input;
using Garant.Platform.Commerce.Models.Garant.Output;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;
using Garant.Platform.Models.Commerce.Input;
using Garant.Platform.Models.Commerce.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HoldPaymentInput = Garant.Platform.Commerce.Models.Garant.Input.HoldPaymentInput;

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
        private readonly ITinkoffService _tinkoffService;

        public GarantController(IGarantActionService _garantActionervice, ICustomerService customerService, IVendorService vendorService, ITinkoffService tinkoffService)
        {
            _garantActionService = _garantActionervice;
            _customerService = customerService;
            _vendorService = vendorService;
            _tinkoffService = tinkoffService;
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
            var result = await _garantActionService.GetInitDataGarantAsync(paymentActionInput.OriginalId, paymentActionInput.OrderType, GetUserName(), paymentActionInput.Stage, paymentActionInput.IsChat);

            return Ok(result);
        }

        /// <summary>
        /// Метод проведет платеж за этап итерации.
        /// </summary>
        /// <param name="holdPaymentInput">Входная модель.</param>
        [HttpPost]
        [Route("payment-iteration-customer")]
        [ProducesResponseType(200, Type = typeof(PaymentInitOutput))]
        public async Task<IActionResult> PaymentIterationCustomerAsync([FromBody] HoldPaymentInput holdPaymentInput)
        {
            var result = await _customerService.PaymentIterationCustomerAsync(holdPaymentInput.OriginalId, GetUserName(), holdPaymentInput.OrderType, holdPaymentInput.Iteration);

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

        /// <summary>
        /// Метод проверит статус платежа.
        /// </summary>
        /// <param name="statePaymentInput">Входная модель.</param>
        /// <returns>Данные платежа.</returns>
        [HttpPost]
        [Route("get-state-payment")]
        [ProducesResponseType(200, Type = typeof(GetPaymentStatusOutput))]
        public async Task<IActionResult> GetStatePaymentAsync([FromBody] StatePaymentInput statePaymentInput)
        {
            var result = await _tinkoffService.GetStatePaymentAsync(statePaymentInput.PaymentId, statePaymentInput.OrderId);

            return Ok(result);
        }

        /// <summary>
        /// Метод выполнит платеж на счет продавца за этап.
        /// </summary>
        [HttpPost]
        [Route("payment-vendor-iteration")]
        public async Task<IActionResult> PaymentVendorIterationAsync()
        {
            return Ok();
        }
    }
}
