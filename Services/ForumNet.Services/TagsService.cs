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

    public class TagsService : ITagsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public TagsService(
            ForumDbContext db,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string name)
        {
            var tag = new Tag
            {
                Name = name,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.Tags.AddAsync(tag);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await this.db.Tags.FirstOrDefaultAsync(t => t.Id == id);

            tag.IsDeleted = true;
            tag.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> AreExisting(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var isExisting = await this.db.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);
                if (!isExisting)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<TModel> GetById<TModel>(int id)
        {
            var tag = await this.db.Tags
                .Where(t => t.Id == id && !t.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return tag;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>()
        {
            var tags = await this.db.Tags
                .Where(t => !t.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return tags;
        }

        public async Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId)
        {
            var tags = await this.db.PostsTags
                .Where(pt => pt.PostId == postId)
                .Select(pt => pt.Tag)
                .Where(t => !t.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return tags;
        }
    }
}