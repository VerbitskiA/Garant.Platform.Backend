using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей категорий франшиз Franchises.FranchiseSubCategories.
    /// </summary>
    [Table("FranchiseSubCategories", Schema = "Franchises")]
    public class FranchiseSubCategoryEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int FranchiseSubCategoryId { get; set; }

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
        [Column("FranchiseSubCategoryCode", TypeName = "varchar(100)")]
        public string FranchiseSubCategoryCode { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        [Column("FranchiseSubCategoryName", TypeName = "varchar(200)")]
        public string FranchiseSubCategoryName { get; set; }
    }
}
