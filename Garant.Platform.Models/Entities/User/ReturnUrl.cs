using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Models.Entities.User
{
    /// <summary>
    /// Класс сопоставляется с таблицей url-возвратов.
    /// </summary>
    [Keyless]
    [Table("ReturnUrls", Schema = "dbo")]
    public class ReturnUrl
    {
        public int UrlId { get; set; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        [Column("Link", TypeName = "text")]
        public string Link { get; set; }

        /// <summary>
        /// Тип ссылки.
        /// </summary>
        [Column("TypeLink", TypeName = "varchar(200)")]
        public string TypeLink { get; set; }
    }
}
