using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Chat
{
    /// <summary>
    /// Контроллер работы с чатом и сообщениями.
    /// </summary>
    [ApiController]
    [Route("chat")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : BaseController
    {
        public ChatController()
        {

        }
    }
}
