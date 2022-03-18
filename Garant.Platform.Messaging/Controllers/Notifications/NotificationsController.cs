using Garant.Platform.Base;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Messaging.Controllers.Notifications
{
    /// <summary>
    /// Контроллер работы с уведомлениями.
    /// </summary>
    [ApiController]
    [Route("notify")]
    public class NotificationsController : BaseController
    {
        public NotificationsController()
        {
        }
    }
}