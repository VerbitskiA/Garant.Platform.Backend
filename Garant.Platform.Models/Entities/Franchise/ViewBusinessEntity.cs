using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей видов бизнеса франшиз Franchises.ViewBusiness.
    /// </summary>
    [Table("ViewBusiness", Schema = "Franchises")]
    public class ViewBusinessEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int ViewBusinessId { get; set; }

        /// <summary>
        /// Guid код вида бизнеса.
        /// </summary>
        [Column("ViewCode", TypeName = "varchar(100)")]
        public Guid ViewCode { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        [Column("ViewName", TypeName = "varchar(200)")]
        public string ViewName { get; set; }
    }
}
