namespace ForumNet.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Common;
    using Models;

    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> tag)
        {
            tag
                .Property(t => t.Name)
                .HasMaxLength(DataConstants.TagNameMaxLength)
                .IsRequired();

            tag
                .HasIndex(t => t.IsDeleted);
        }
    }
}
