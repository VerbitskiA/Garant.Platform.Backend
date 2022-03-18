using Garant.Platform.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Base
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Метод получит имя текущего юзера.
        /// </summary>
        /// <returns>Логин пользователя.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected string GetUserName()
        {
            // Запишет логин в куки и вернет фронту.
            if (!HttpContext.Request.Cookies.ContainsKey("name"))
            {
                HttpContext.Response.Cookies.Append("user", HttpContext?.User?.Identity?.Name ?? string.Empty);
            }

            return HttpContext?.User?.Identity?.Name ?? GetLoginFromCookie();
        }

        /// <summary>
        /// Метод вернет логин пользователя из куки.
        /// </summary>  
        /// <returns>Логин пользователя.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        private string GetLoginFromCookie()
        {
            return HttpContext.Request.Cookies["user"];
        }

        // [ApiExplorerSettings(IgnoreApi = true)]
        // public string GetGeolocationFromCookies()
        // {
        //     if (HttpContext == null)
        //     {
        //         return LocationTypeEnum.RU.ToString();
        //     }
        //     
        //     if (HttpContext.Request.Cookies["geozone"]!.Equals(LocationTypeEnum.RU.ToString()))
        //     {
        //         return LocationTypeEnum.RU.ToString();
        //     }
        //     
        //     if (HttpContext.Request.Cookies["geozone"]!.Equals(LocationTypeEnum.EN.ToString()))
        //     {
        //         return LocationTypeEnum.EN.ToString();
        //     }
        //
        //     return LocationTypeEnum.RU.ToString();
        // }
    }
}
