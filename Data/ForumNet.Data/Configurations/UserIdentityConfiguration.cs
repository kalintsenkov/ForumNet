namespace ForumNet.Data.Configurations
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserIdentityConfiguration : IEntityTypeConfiguration<ForumUser>
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

            user
                .HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            user
                .HasMany(u => u.Logins)
                .WithOne()
                .HasForeignKey(l => l.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            user
                .HasMany(u => u.Roles)
                .WithOne()
                .HasForeignKey(l => l.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}