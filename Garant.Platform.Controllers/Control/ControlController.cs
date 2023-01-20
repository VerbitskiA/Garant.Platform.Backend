using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Control;
using Garant.Platform.Base;
using Garant.Platform.Models.Control.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Control
{
    /// <summary>
    /// Контроллер для работы с разными контролами.
    /// </summary>
    [ApiController, Route("control")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ControlController : BaseController
    {
        private readonly IControlService _controlService;

        public ControlController(IControlService controlService)
        {
            _controlService = controlService;
        }

        /// <summary>
        /// Метод получит список названий банков для профиля.
        /// </summary>
        /// <returns>Список названий банков.</returns>
        [HttpPost]
        [Route("get-bank-names-list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ControlOutput>))]
        public async Task<IActionResult> GetFilterBankNameValuesAsync()
        {
            var result = await _controlService.GetFilterBankNameValuesAsync(GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод найдет банки по их названию.
        /// </summary>
        /// <returns>Список названий банков.</returns>
        [HttpGet]
        [Route("search-bank-name")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ControlOutput>))]
        public async Task<IActionResult> SearchFilterBankNameValueAsync([FromQuery] string searchText)
        {
            var result = await _controlService.SearchFilterBankNameValueAsync(searchText);

            return Ok(result);
        }
    }
}
