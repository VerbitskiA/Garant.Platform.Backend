using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей блогов Info.BlogThemes. 
    /// </summary>
    [Table("BlogThemes", Schema = "Info")]
    public class BlogThemeEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long BlogThemeId { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Код категории темы блога (GUID).
        /// </summary>
        [Column("ThemeCategoryCode", TypeName = "varchar(100)")]
        public string ThemeCategoryCode { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [Column("DateCreated", TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }
    }
}