using Garant.Platform.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Messaging.Controllers.Notifications
{
    /// <summary>
    /// Контроллер работы с уведомлениями.
    /// </summary>
    [ApiController]
    [Route("notify")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsController : BaseController
    {
        public NotificationsController()
        {
        }
    }
}