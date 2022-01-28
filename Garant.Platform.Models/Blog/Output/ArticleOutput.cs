using System;

namespace Garant.Platform.Models.Blog.Output
{
    public class ArticleOutput
    {        
        public string Urls { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Text { get; set; }
        
        public DateTime DateCreated { get; set; }

        public int Position { get; set; }
        public Guid ArticleCode { get; set; }
    }
}
