using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Footer
{
    /// <summary>
    /// Класс сопоставляется с таблицей dbo.Footer.
    /// </summary>
    [Table("Footer", Schema = "dbo")]
    public class FooterEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key] 
        public int FooterId { get; set; }

        /// <summary>
        /// Название заголовка столбца.
        /// </summary>
        [Column("FooterTitle", TypeName = "varchar(200)")]
        public string FooterTitle { get; set; }

        /// <summary>
        /// Название поля.
        /// </summary>
        [Column("FooterFieldName", TypeName = "varchar(200)")]
        public string FooterFieldName { get; set; }

        /// <summary>
        /// Флаг кнопки разместить заявление.
        /// </summary>
        [Column("IsPlace", TypeName = "bool")]
        public bool IsPlace { get; set; }

        /// <summary>
        /// Флаг одиночного заголовка без других полей.
        /// </summary>
        [Column("IsSignleTitle", TypeName = "bool")]
        public bool IsSignleTitle { get; set; }

        /// <summary>
        /// Номер столбца.
        /// </summary>
        [Column("Column", TypeName = "int")]
        public int Column { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}
