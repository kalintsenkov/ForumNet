namespace ForumNet.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> category)
        {
            category
                .Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            category
                .HasIndex(c => c.IsDeleted);
        }
    }
}