using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей категорий бизнеса франшиз Franchises.Categories.
    /// </summary>
    [Table("Categories", Schema = "Franchises")]
    public class CategoryEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Guid код категории бизнеса.
        /// </summary>
        [Column("CategoryCode", TypeName = "varchar(100)")]
        public Guid CategoryCode { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        [Column("CategoryName", TypeName = "varchar(200)")]
        public string CategoryName { get; set; }
    }
}
