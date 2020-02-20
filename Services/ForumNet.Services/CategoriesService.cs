namespace ForumNet.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
   
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    
    using Contracts;
    using Data;
    using Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;

        public CategoriesService(ForumDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task Create(string name)
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

        public async Task Delete(int id)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            category.IsDeleted = true;
            category.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetById<TModel>(int id)
        {
            var category = await this.db.Categories
                .Where(c => c.Id == id)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return category;
        }
    }
}