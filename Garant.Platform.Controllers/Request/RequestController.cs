using System.Collections.Generic;
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
        public async Task<RequestFranchiseOutput> CreateRequestFranchiseAsync([FromBody] RequestFranchiseInput requestFranchiseInput)
        {
            var result = await _requestService.CreateRequestFranchiseAsync(requestFranchiseInput.UserName, requestFranchiseInput.Phone, requestFranchiseInput.City, GetUserName(), requestFranchiseInput.FranchiseId);

            return result;
        }

        /// <summary>
        /// Метод создаст заявку бизнеса.
        /// </summary>
        /// <param name="requestFranchiseInput">Входная модель.</param>
        /// <returns>Данные заявки.</returns>
        [HttpPost]
        [Route("create-request-business")]
        [ProducesResponseType(200, Type = typeof(RequestBusinessOutput))]
        public async Task<RequestBusinessOutput> CreateRequestBusinessAsync([FromBody] RequestBusinessInput requestBusinessInput)
        {
            var result = await _requestService.CreateRequestBusinessAsync(requestBusinessInput.UserName, requestBusinessInput.Phone, GetUserName(), requestBusinessInput.BusinessId);

            return result;
        }

        /// <summary>
        /// Метод получит список заявок для вкладки профиля "Уведомления".
        /// </summary>
        /// <returns>Список заявок.</returns>
        [HttpGet]
        [Route("get-requests")]
        [ProducesResponseType(200, Type = typeof(RequestBusinessOutput))]
        public async Task<IEnumerable<RequestBusinessOutput>> GetBusinessRequestsAsync()
        {
            var result = await _requestService.GetBusinessRequestsAsync(GetUserName());

            return result;
        }
    }
}
