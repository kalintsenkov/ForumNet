namespace ForumNet.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ForumDbContext : DbContext
    {
        public ForumDbContext()
        {
        }

        public ForumDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostTag> PostsTags { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(DataSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}