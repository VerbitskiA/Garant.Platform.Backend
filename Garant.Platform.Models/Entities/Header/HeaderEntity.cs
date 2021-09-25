using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Header
{
    /// <summary>
    /// Класс сопоставляется с таблицей dbo.Headers.
    /// </summary>
    [Table("Headers", Schema = "dbo")]
    public class HeaderEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key, Column("HeaderId", TypeName = "int")]
        public int HeaderId { get; set; }

        /// <summary>
        /// Название поля хидера.
        /// </summary>
        [Column("HeaderName", TypeName = "varchar(200)")]
        public string HeaderName { get; set; }

        /// <summary>
        /// Тип хидера.
        /// </summary>
        [Column("HeaderType", TypeName = "varchar(50)")]
        public string HeaderType { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}
