using System;
using System.Collections.Generic;
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
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }

        /// <summary>
        /// Идентификатор темы блога.
        /// </summary>
        [Column("ThemeCategoryCode", TypeName = "varchar(100)")]
        public string ThemeCategoryCode { get; set; }

        /// <summary>
        /// Дата создания блога
        /// </summary>
        [Column("DateCreated", TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Количество просмотров.
        /// </summary>
        [Column("ViewsCount", TypeName = "bigint")]
        public long ViewsCount { get; set; }

        /// <summary>
        /// Статьи блога.
        /// </summary>
        public List<ArticleEntity> Articles { get; set; }
    }
}
