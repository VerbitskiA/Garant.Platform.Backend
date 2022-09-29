using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей Info.ArticlesViews.
    /// </summary>
    [Table("ArticlesViews", Schema = "Info")]
    public class ArticlesViewsEntity
    {
        /// <summary>
        /// Идентификатор статьи.
        /// </summary>        
        public long ArticleId { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>        
        public string UserId { get; set; }

        /// <summary>
        /// Дата просмотра.
        /// </summary>
        [Column("ViewDate", TypeName = "timestamp")]
        public DateTime ViewDate { get; set; }
    }
}
