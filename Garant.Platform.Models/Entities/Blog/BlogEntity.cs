using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей блогов Info.Blogs. 
    /// </summary>
    [Table("Blogs",Schema = "Info")]
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

        /// <summary>
        /// Позиция при размещении.
        /// </summary>
        [Column("Position", TypeName = "int4")]
        public int Position { get; set; }

        /// <summary>
        /// FK. Идентификатор темы блога.
        /// </summary>
        [ForeignKey("Blogs_BlogThemeId_fkey")]
        [Column("BlogThemeId", TypeName = "bigserial")]
        public long BlogThemeId { get; set; }

        /// <summary>
        /// Дата создания блога
        /// </summary>
        [Column("DateCreated", TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }

        public BlogThemeEntity BlogTheme { get; set; }
    }
}
