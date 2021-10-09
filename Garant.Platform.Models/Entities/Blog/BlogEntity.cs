using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей блогов dbo.Blogs. 
    /// </summary>
    [Table("Blogs",Schema = "dbo")]
    public class BlogEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long BlogId { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        [Column("Url", TypeName = "text")]
        public string Url { get; set; }

        /// <summary>
        /// Оплачено ли размещение на главной.
        /// </summary>
        [Column("IsPaid", TypeName = "bool")]
        public bool IsPaid { get; set; }
    }
}
