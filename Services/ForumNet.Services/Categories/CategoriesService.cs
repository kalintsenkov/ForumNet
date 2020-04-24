namespace ForumNet.Services.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Providers.DateTime;

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

        public async Task CreateAsync(string name)
        {
            var category = new Category
            {
                Name = name,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string name)
        {
            var category = await this.GetByIdAsync(id);

            category.Name = name;
            category.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await this.GetByIdAsync(id);

            category.IsDeleted = true;
            category.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExistingAsync(int id) 
            => await this.db.Categories.AnyAsync(c => c.Id == id && !c.IsDeleted);

        public async Task<bool> IsExistingAsync(string name) 
            => await this.db.Categories.AnyAsync(c => c.Name == name && !c.IsDeleted);

        public async Task<TModel> GetByIdAsync<TModel>(int id) 
            => await this.db.Categories
                .AsNoTracking()
                .Where(c => c.Id == id && !c.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null)
        {
            var queryable = this.db.Categories
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(q => q.Name.Contains(search));
            }

            var categories = await queryable
                .OrderByDescending(c => c.Posts.Count)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return categories;
        }

        private async Task<Category> GetByIdAsync(int id)
            => await this.db.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }
}