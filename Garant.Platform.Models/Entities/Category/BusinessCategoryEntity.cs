using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Category
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
        /// Название категории.
        /// </summary>
        [Column("Name", TypeName = "varchar(200)")]
        public string Name { get; set; }
    }
}
