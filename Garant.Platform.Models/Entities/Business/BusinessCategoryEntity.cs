using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Business
{
    /// <summary>
    /// Класс сопоставляется с таблицей категорий бизнеса Business.BusinessCategories.
    /// </summary>
    [Table("BusinessCategories", Schema = "Business")]
    public class BusinessCategoryEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

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
        /// Код категории бизнеса.
        /// </summary>
        [Column("BusinessCategoryCode", TypeName = "varchar(100)")]
        public string BusinessCategoryCode { get; set; }

        /// <summary>
        /// Название категории бизнеса.
        /// </summary>
        [Column("BusinessCategoryName", TypeName = "varchar(200)")]
        public string BusinessCategoryName { get; set; }

        [Column("FranchiseCategorySysName", TypeName = "varchar(150)")]
        public string FranchiseCategorySysName { get; set; }
    }
}
