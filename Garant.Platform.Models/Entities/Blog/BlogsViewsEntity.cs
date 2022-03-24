using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей Info.BlogsViews.
    /// </summary>
    [Table("BlogsViews", Schema = "Info")]
    public class BlogsViewsEntity
    {
        /// <summary>
        /// Идентификатор блога.
        /// </summary>        
        public long BlogId { get; set; }

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
