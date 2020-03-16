namespace ForumNet.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(ForumDbContext dbContext, IServiceProvider serviceProvider);
    }
}