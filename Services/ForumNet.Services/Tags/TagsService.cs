﻿namespace ForumNet.Services.Tags
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

    public class TagsService : ITagsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public TagsService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
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
            var tag = await this.db.Tags.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            tag.IsDeleted = true;
            tag.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExistingAsync(int id) 
            => await this.db.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);

        public async Task<bool> IsExistingAsync(string name) 
            => await this.db.Tags.AnyAsync(t => t.Name == name && !t.IsDeleted);

        public async Task<bool> AreExistingAsync(IEnumerable<int> ids)
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

        public async Task<int> GetCountAsync(string searchFilter = null)
        {
            var queryable = this.db.Tags
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryable = queryable
                    .Where(t => t.Name.Contains(searchFilter));
            }

            var count = await queryable.CountAsync();

            return count;
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.db.Tags
                .AsNoTracking()
                .Where(t => t.Id == id && !t.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null, int skip = 0, int? take = null)
        {
            var queryable = this.db.Tags
                .AsNoTracking()
                .OrderByDescending(t => t.Posts
                    .Count(p => !p.Post.IsDeleted))
                .Where(t => !t.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(t => t.Name.Contains(search));
            }

            if (take.HasValue)
            {
                queryable = queryable.Skip(skip).Take(take.Value);
            }

            var tags = await queryable
                 .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                 .ToListAsync();

            return tags;
        }

        public async Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId) 
            => await this.db.PostsTags
                .AsNoTracking()
                .Where(pt => pt.PostId == postId && !pt.Post.IsDeleted)
                .Select(pt => pt.Tag)
                .Where(t => !t.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}