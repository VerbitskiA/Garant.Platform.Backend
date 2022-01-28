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
        [ForeignKey("Articles_BlogId_fkey")]
        public long BlogId { get; set; }        

        /// <summary>
        /// Путь к изображениям.
        /// </summary>
        [Column("Urls", TypeName = "text")]
        public string Urls { get; set; }

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
        [Column("Position", TypeName = "int4")]
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
        public Guid ArticleCode { get; set; }
    }
}
