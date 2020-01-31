namespace ForumNet.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ReplyReportConfiguration: IEntityTypeConfiguration<ReplyReport>
    {
        public void Configure(EntityTypeBuilder<ReplyReport> replyReport)
        {
            replyReport
                .Property(rr => rr.Description)
                .HasMaxLength(1000)
                .IsRequired();

            replyReport
                .HasOne(rr => rr.Author)
                .WithMany(a => a.ReplyReports)
                .HasForeignKey(rr => rr.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            replyReport
                .HasOne(rr => rr.Reply)
                .WithMany(r => r.Reports)
                .HasForeignKey(rr => rr.ReplyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}