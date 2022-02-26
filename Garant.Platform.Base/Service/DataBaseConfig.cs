// using Garant.Platform.Base.Abstraction;
// using Garant.Platform.Core.Data;
// using Garant.Platform.Core.Enums;
//
// namespace Garant.Platform.Base.Service
// {
//     /// <summary>
//     /// Класс реализует методы сервиса настроек БД.
//     /// </summary>
//     public class DataBaseConfig : BaseController, IDataBaseConfig
//     {
//         private readonly string CONN_STR_RU = ConnectionTypeEnum.NpgTestSqlConnectionRu.ToString();
//         // private string CONN_STR = ConnectionTypeEnum.NpgConfigurationConnectionRu.ToString();
//         private readonly string CONN_STR_EN = ConnectionTypeEnum.NpgTestSqlConnectionEn.ToString();
//         // private string CONN_STR = ConnectionTypeEnum.NpgConfigurationConnectionEn.ToString();
//         
//         /// <summary>
//         /// Метод определит, какой датаконтекст использовать. Это зависит от геолокации пользователя.
//         /// </summary>
//         /// <returns>Датаконтекст.</returns>
//         public PostgreDbContext GetDbContext()
//         {
//             var geozone = GetGeolocationFromCookies();
//             
//             if (string.IsNullOrEmpty(geozone))
//             {
//                 return DbContextFactory.CreateDbContext(CONN_STR_RU);
//             }
//
//             if (geozone.Equals("ru"))
//             {
//                 return DbContextFactory.CreateDbContext(CONN_STR_RU);
//             }
//                 
//             if (geozone.Equals("en"))
//             {
//                 return DbContextFactory.CreateDbContext(CONN_STR_EN);
//             }
//             
//             return DbContextFactory.CreateDbContext(CONN_STR_RU);
//         }
//
//         /// <summary>
//         /// Метод определит, какой датаконтекст использовать. Это зависит от геолокации пользователя.
//         /// </summary>
//         /// <returns>Датаконтекст.</returns>
//         public IdentityDbContext GetIdentityDbContext()
//         {
//             var geozone = GetGeolocationFromCookies();
//             
//             if (string.IsNullOrEmpty(geozone))
//             {
//                 return DbContextFactory.CreateIdentityDbContext(CONN_STR_RU);
//             }
//
//             if (geozone.Equals("ru"))
//             {
//                 return DbContextFactory.CreateIdentityDbContext(CONN_STR_RU);
//             }
//                 
//             if (geozone.Equals("en"))
//             {
//                 return DbContextFactory.CreateIdentityDbContext(CONN_STR_EN);
//             }
//             
//             return DbContextFactory.CreateIdentityDbContext(CONN_STR_RU);
//         }
//     }
// }