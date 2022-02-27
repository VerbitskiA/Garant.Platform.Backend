using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Business
{
    /// <summary>
    /// Класс сопоставляется с таблицей категорий бизнеса Business.BusinessSubCategories.
    /// </summary>
    [Table("BusinessSubCategories", Schema = "Business")]
    public class BusinessSubCategoryEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int BusinessSubCategoryId { get; set; }

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
        [Column("BusinessSubCategoryCode", TypeName = "varchar(100)")]
        public string BusinessSubCategoryCode { get; set; }

        /// <summary>
        /// Название категории бизнеса.
        /// </summary>
        [Column("BusinessSubCategoryName", TypeName = "varchar(200)")]
        public string BusinessSubCategoryName { get; set; }
        
        /// <summary>
        /// Системное название сферы.
        /// </summary>
        [Column("BusinessCategorySysName", TypeName = "varchar(150)")]
        public string BusinessCategorySysName { get; set; }

        /// <summary>
        /// Системное название подкатегории.
        /// </summary>
        [Column("BusinessSubCategorySysName", TypeName = "varchar(150)")]
        public string BusinessSubCategorySysName { get; set; }

        /// <summary>
        /// Код категории.
        /// </summary>
        [Column("BusinessCategoryCode", TypeName = "varchar(150)")]
        public string BusinessCategoryCode { get; set; }
    }
}
