using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.News
{
    /// <summary>
    /// Класс сопоставляется с таблицей Info.NewsViews.
    /// </summary>
    [Table("NewsViews", Schema = "Info")]
    public class NewsViewsEntity
    {
        /// <summary>
        /// Идентификатор новости.
        /// </summary>        
        public long NewsId { get; set; }

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
