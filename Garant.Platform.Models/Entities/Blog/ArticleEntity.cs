using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Garant.Platform.Models.Entities.Blog
{
    [Table("Articles", Schema = "Info")]
    public class ArticleEntity
    {
        [Key]
        public long ArticleId { get; set; }

        [ForeignKey("Articles_BlogId_fkey")]
        public long BlogId { get; set; }        

        [Column("Urls", TypeName = "text")]
        public string Urls { get; set; }

        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        [Column("Description", TypeName = "varchar(400)")]
        public string Description { get; set; }

        [Column("Text", TypeName = "text")]
        public string Text { get; set; }

        [Column("Position", TypeName = "int4")]
        public int Position { get; set; }

        [Column("DateCreated", TypeName = "timestamp")]
        public DateTime DateCreated { get; set; }
        
        [Column("ArticleCode", TypeName = "nvarchar(100)")]
        public Guid ArticleCode { get; set; }
    }
}
