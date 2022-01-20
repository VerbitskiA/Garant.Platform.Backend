using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей сфер франшиз Franchises.FranchiseCategories.
    /// </summary>
    [Table("FranchiseCategories", Schema = "Franchises")]
    public class FranchiseCategoryEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int FranchiseCategoryId { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        [Column("Url", TypeName = "text")]
        public string Url { get; set; }

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

        /// <summary>
        /// Guid код категории бизнеса.
        /// </summary>
        [Column("CategoryCode", TypeName = "varchar(100)")]
        public string FranchiseCategoryCode { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        [Column("CategoryName", TypeName = "varchar(200)")]
        public string FranchiseCategoryName { get; set; }
    }
}
