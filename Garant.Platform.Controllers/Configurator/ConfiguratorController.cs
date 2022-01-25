using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Configurator
{
    /// <summary>
    /// Контроллер для работы с конфигуратором.
    /// </summary>
    [ApiController, Route("configurato")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ConfiguratorController : BaseController
    {
        public ConfiguratorController()
        {
            
        }
    }
}