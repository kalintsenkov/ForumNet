namespace ForumNet.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
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