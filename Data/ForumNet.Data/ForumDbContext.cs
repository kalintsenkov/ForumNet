namespace ForumNet.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    
    using Models;

    public class ForumDbContext : IdentityDbContext<ForumUser, ForumRole, string>
    {
        public ForumDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostReport> PostReports { get; set; }

        public DbSet<PostTag> PostsTags { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<ReplyReport> ReplyReports { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}