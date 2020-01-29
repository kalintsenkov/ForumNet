namespace ForumNet.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> postTag)
        {
            postTag.HasKey(pt => new { pt.PostId, pt.TagId });

            postTag
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostsTags)
                .HasForeignKey(pt => pt.PostId);

            postTag
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostsTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}