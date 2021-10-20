using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Header
{
    /// <summary>
    /// Класс сопоставляется с таблицей хлебных крошек dbo.Breadcrumbs.
    /// </summary>
    [Table("Breadcrumbs", Schema = "dbo")]
    public class BreadcrumbEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int BreadcrumbId { get; set; }

        /// <summary>
        /// Название пункта.
        /// </summary>
        [Column("Label", TypeName = "varchar(100)")]
        public string Label { get; set; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        [Column("Url", TypeName = "varchar(400)")]
        public string Url { get; set; }

        /// <summary>
        /// Селектор страницы, для которой нужно сформировать хлебные крошки.
        /// </summary>
        [Column("SelectorPage", TypeName = "varchar(100)")]
        public string SelectorPage { get; set; }

        /// <summary>
        /// Номер позиции в списке.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}
