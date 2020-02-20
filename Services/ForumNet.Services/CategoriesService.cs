namespace ForumNet.Services
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly ForumDbContext db;

        public CategoriesService(ForumDbContext db)
        {
            this.db = db;
        }

        public async Task Add(string name)
        {
            var category = new Category
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();
        }
    }
}