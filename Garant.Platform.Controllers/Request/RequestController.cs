using System.Threading.Tasks;
using Garant.Platform.Abstractions.Request;
using Garant.Platform.Base;
using Garant.Platform.Models.Request.Input;
using Garant.Platform.Models.Request.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Request
{
    /// <summary>
    /// Контроллер заявок.
    /// </summary>
    [ApiController]
    [Route("request")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestController : BaseController
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Метод создаст заявку франшизы.
        /// </summary>
        /// <param name="requestFranchiseInput">Входная модель.</param>
        /// <returns>Данные заявки.</returns>
        [HttpPost]
        [Route("create-request-franchise")]
        [ProducesResponseType(200, Type = typeof(RequestFranchiseOutput))]
        public async Task<IActionResult> CreateRequestFranchiseAsync([FromBody] RequestFranchiseInput requestFranchiseInput)
        {
            var result = await _requestService.CreateRequestFranchiseAsync(requestFranchiseInput.UserName, requestFranchiseInput.Phone, requestFranchiseInput.City, GetUserName(), requestFranchiseInput.FranchiseId);

            return Ok(result);
        }

        /// <summary>
        /// Метод создаст заявку бизнеса.
        /// </summary>
        /// <param name="requestFranchiseInput">Входная модель.</param>
        /// <returns>Данные заявки.</returns>
        [HttpPost]
        [Route("create-request-business")]
        [ProducesResponseType(200, Type = typeof(RequestBusinessOutput))]
        public async Task<IActionResult> CreateRequestBusinessAsync([FromBody] RequestBusinessInput requestBusinessInput)
        {
            var result = await _requestService.CreateRequestBusinessAsync(requestBusinessInput.UserName, requestBusinessInput.Phone, GetUserName(), requestBusinessInput.BusinessId);

            return Ok(result);
        }

        /// <summary>
        /// Метод проверит, подтверждена ли заявка.
        /// </summary>
        /// <param name="id">Id франшизы или бизнеса.</param>
        /// <param name="type">Тип франшиза или бизнес.</param>
        /// <returns>Статус проверки.</returns>
        [HttpGet]
        [Route("check-confirmed-request")]
        public async Task<bool> CheckConfirmedRequestAsync([FromQuery] string id, string type)
        {
            var result = await _requestService.CheckConfirmedRequestAsync(id, type, GetUserName());

            return result;
        }
    }
}
