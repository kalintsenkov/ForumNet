namespace ForumNet.Data.Configurations
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<ForumUser>
    {
        public void Configure(EntityTypeBuilder<ForumUser> user)
        {
            user
                .Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            user
                .Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            user
                .Property(u => u.Biography)
                .HasMaxLength(250);
        }
    }
}