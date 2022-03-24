using Garant.Platform.Models.Entities.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Garant.Platform.Models.Mappings.News
{
    public partial class ArticlesViewsEntityConfiguration : IEntityTypeConfiguration<ArticlesViewsEntity>
    {
        public void Configure(EntityTypeBuilder<ArticlesViewsEntity> builder)
        {
            builder.ToTable("ArticlesViews", "Info");
            builder.HasKey(c => new { c.ArticleId, c.UserId });
        }
    }
}
