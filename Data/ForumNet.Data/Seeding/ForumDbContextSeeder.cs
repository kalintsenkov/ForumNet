namespace ForumNet.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ForumDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new AdminSeeder(),
                new TestUserSeeder(),
                new TagsSeeder(),
                new CategoriesSeeder(),
                new PostsSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}