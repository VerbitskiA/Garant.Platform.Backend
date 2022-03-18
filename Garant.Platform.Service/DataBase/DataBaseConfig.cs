using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Base;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Enums;
using Garant.Platform.Core.Utils;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Services.DataBase
{
    /// <summary>
    /// Класс реализует методы сервиса настроек БД.
    /// </summary>
    public class DataBaseConfig : BaseController, IDataBaseConfig
    {
        // private readonly string CONN_STR_RU = ConnectionTypeEnum.NpgTestSqlConnectionRu.ToString();
        private readonly string CONN_STR_RU = ConnectionTypeEnum.NpgConfigurationConnectionRu.ToString();
        // private readonly string CONN_STR_EN = ConnectionTypeEnum.NpgTestSqlConnectionEn.ToString();
        private readonly string CONN_STR_EN = ConnectionTypeEnum.NpgConfigurationConnectionEn.ToString();
        
        /// <summary>
        /// Метод определит, какой датаконтекст использовать. Это зависит от геолокации пользователя.
        /// </summary>
        /// <returns>Датаконтекст.</returns>
        public PostgreDbContext GetDbContext()
        {
            var context = AutoFac.Resolve<IHttpContextAccessor>();
            var geozone = context.HttpContext?.Request.Cookies["geozone"];
            
            if (string.IsNullOrEmpty(geozone))
            {
                return DbContextFactory.CreateDbContext(CONN_STR_RU);
            }

            if (geozone.Equals("ru"))
            {
                return DbContextFactory.CreateDbContext(CONN_STR_RU);
            }
                
            if (geozone.Equals("en"))
            {
                return DbContextFactory.CreateDbContext(CONN_STR_EN);
            }
            
            return DbContextFactory.CreateDbContext(CONN_STR_RU);
        }

        /// <summary>
        /// Метод определит, какой датаконтекст использовать. Это зависит от геолокации пользователя.
        /// </summary>
        /// <returns>Датаконтекст.</returns>
        public IdentityDbContext GetIdentityDbContext()
        {
            var context = AutoFac.Resolve<IHttpContextAccessor>();
            var geozone = context.HttpContext?.Request.Cookies["geozone"];
            
            if (string.IsNullOrEmpty(geozone))
            {
                return DbContextFactory.CreateIdentityDbContext(CONN_STR_RU);
            }

            if (geozone.Equals("ru"))
            {
                return DbContextFactory.CreateIdentityDbContext(CONN_STR_RU);
            }
                
            if (geozone.Equals("en"))
            {
                return DbContextFactory.CreateIdentityDbContext(CONN_STR_EN);
            }
            
            return DbContextFactory.CreateIdentityDbContext(CONN_STR_RU);
        }
    }
}