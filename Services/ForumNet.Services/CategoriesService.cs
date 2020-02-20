namespace ForumNet.Services
{
    using System.Collections.Generic;
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
        private readonly IDateTimeProvider dateTimeProvider;

        public CategoriesService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task Create(string name)
        {
            var category = new Category
            {
                Name = name,
                CreatedOn = this.dateTimeProvider.Now(),
                ModifiedOn = this.dateTimeProvider.Now()
            };

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await this.db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            category.IsDeleted = true;
            category.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetById<TModel>(int id)
        {
            var category = await this.db.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<IEnumerable<TModel>> GetAll<TModel>()
        {
            var categories = await this.db.Categories
                .Where(c => !c.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return categories;
        }
    }
}