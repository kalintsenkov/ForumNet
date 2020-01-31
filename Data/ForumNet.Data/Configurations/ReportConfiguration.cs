namespace ForumNet.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> report)
        {
            report
                .Property(r => r.Description)
                .HasMaxLength(1000)
                .IsRequired();

            report
                .HasOne(r => r.Author)
                .WithMany(a => a.Reports)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            report
                .HasOne(r => r.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}