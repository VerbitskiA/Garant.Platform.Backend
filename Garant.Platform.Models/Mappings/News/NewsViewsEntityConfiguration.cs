using Garant.Platform.Models.Entities.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Garant.Platform.Models.Mappings.News
{
    public partial class NewsViewsEntityConfiguration : IEntityTypeConfiguration<NewsViewsEntity>
    {
        public void Configure(EntityTypeBuilder<NewsViewsEntity> builder)
        {
            builder.ToTable("NewsViews", "Info");
            builder.HasKey(c => new { c.NewsId, c.UserId });
        }
    }
}
