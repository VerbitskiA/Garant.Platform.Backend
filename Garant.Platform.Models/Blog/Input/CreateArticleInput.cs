using System;

namespace Garant.Platform.Models.Blog.Input
{
    public class CreateArticleInput
    {
        public long BlogId { get; set; }

        public string Urls { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Text { get; set; }

        public int Position { get; set; }

        public Guid ArticleCode { get; set; }
    }
}
