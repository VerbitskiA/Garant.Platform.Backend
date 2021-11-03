using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.ReadyBusiness
{
    /// <summary>
    /// Контроллер готового бизнеса.
    /// </summary>
    [ApiController]
    [Route("business")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessController : BaseController
    {
        public BusinessController()
        {

        }

        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        [HttpPost]
        [Route("create-update-business")]
        public async Task<IActionResult> CreateUpdateBusinessAsync([FromForm] IFormCollection businessFilesInput, [FromForm] string businessDataInput)
        {
            return Ok();
        }
    }
}
