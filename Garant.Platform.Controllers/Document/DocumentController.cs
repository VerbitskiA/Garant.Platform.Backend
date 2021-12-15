using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Document
{
    /// <summary>
    /// Контроллер работы со всеми документами.
    /// </summary>
    [ApiController, Route("document")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentController : BaseController
    {
        public DocumentController()
        {

        }
    }
}
