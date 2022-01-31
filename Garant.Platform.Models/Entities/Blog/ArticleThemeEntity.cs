using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей списка тем для статей блога.
    /// </summary>
    [Table("ArticleThemes", Schema = "Info")]
    public class ArticleThemeEntity
    {
        [Key] 
        public long ThemeId { get; set; }

        /// <summary>
        /// Код темы статьи.
        /// </summary>
        [Column("ThemeCode", TypeName = "varchar(150)")]
        public Guid ThemeCode { get; set; }

        /// <summary>
        /// Название темы статьи.
        /// </summary>
        [Column("ThemeName", TypeName = "varchar(400)")]
        public string ThemeName { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}