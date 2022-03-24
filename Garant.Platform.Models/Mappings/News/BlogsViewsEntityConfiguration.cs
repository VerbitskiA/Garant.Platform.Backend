using Garant.Platform.Models.Entities.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Garant.Platform.Models.Mappings.News
{
    public partial class BlogsViewsEntityConfiguration : IEntityTypeConfiguration<BlogsViewsEntity>
    {
        public void Configure(EntityTypeBuilder<BlogsViewsEntity> builder)
        {
            builder.ToTable("BlogsViews", "Info");
            builder.HasKey(c => new { c.BlogId, c.UserId });
        }
    }
}
