using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей Info.Articles
    /// </summary>
    [Table("Articles", Schema = "Info")]
    public class ArticleEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long ArticleId { get; set; }

        /// <summary>
        /// FK, идентификатор блога.
        /// </summary>
        public long BlogId { get; set; }

        /// <summary>
        /// Блог.
        /// </summary>
        public BlogEntity Blog { get; set; }

        /// <summary>
        /// Изображение превью облажки.
        /// </summary>
        [Column("PreviewUrl", TypeName = "text")]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Изображение статьи.
        /// </summary>
        [Column("ArticleUrl", TypeName = "text")]
        public string ArticleUrl { get; set; }

        /// <summary>
        /// Заголовок статьи.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Описание статьи.
        /// </summary>
        [Column("Description", TypeName = "varchar(400)")]
        public string Description { get; set; }

        /// <summary>
        /// Текст статьи.
        /// </summary>
        [Column("Text", TypeName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Позиция при размещении.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [Column("DateCreated", TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }
        
        /// <summary>
        /// Код статьи.
        /// </summary>
        [Column("ArticleCode", TypeName = "text")]
        public string ArticleCode { get; set; }

        /// <summary>
        /// Код темы статьи. 
        /// </summary>
        [Column("ThemeCode", TypeName = "varchar(100)")]
        public string ThemeCode { get; set; }

        /// <summary>
        /// Подпись основного изображения статьи.
        /// </summary>
        [Column("SignatureText", TypeName = "varchar(300)")]
        public string SignatureText { get; set; }

        /// <summary>
        /// Количество просмотров.
        /// </summary>
        [Column("ViewsCount", TypeName = "bigint")]
        public long ViewsCount { get; set; }
    }
}
