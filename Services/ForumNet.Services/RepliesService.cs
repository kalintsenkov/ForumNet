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

    public class RepliesService : IRepliesService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public RepliesService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string description, int? parentId, int postId, string authorId)
        {
            var reply = new Reply
            {
                Description = description,
                ParentId = parentId,
                PostId = postId,
                AuthorId = authorId,
                CreatedOn = this.dateTimeProvider.Now(),
            };

            await this.db.Replies.AddAsync(reply);
            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string description)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id);

            reply.Description = description;
            reply.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id);

            reply.IsDeleted = true;
            reply.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExisting(int id)
        {
            return await this.db.Replies.AnyAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<string> GetAuthorIdById(int id)
        {
            var authorId = await this.db.Replies
                .Where(r => r.Id == id && !r.IsDeleted)
                .Select(r => r.AuthorId)
                .FirstOrDefaultAsync();

            return authorId;
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
        {
            var reply = await this.db.Replies
                .Where(r => r.Id == id && !r.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return reply;
        }

        public async Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId)
        {
            var replies = await this.db.Replies
                .Where(r => r.AuthorId == userId && !r.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return replies;
        }

        public async Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId, string sort)
        {
            var queryable = this.db.Replies
                .Where(r => r.PostId == postId && !r.IsDeleted);

            queryable = sort switch
            {
                "recent" => queryable.OrderByDescending(r => r.CreatedOn),
                "most reacted" => queryable.OrderByDescending(r => r.Reactions.Count),
                "longest" => queryable.OrderByDescending(r => r.Description.Length),
                "shortest" => queryable.OrderBy(r => r.Description.Length),
                _ => queryable.OrderByDescending(r => r.CreatedOn)
            };

            var replies = await queryable
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return replies;
        }

        public async Task<IEnumerable<TModel>> GetAllNestedByPostIdAndReplyIdAsync<TModel>(int postId, int? parentId)
        {
            var nestedReplies = await this.db.Replies
                .Where(r => r.PostId == postId && !r.IsDeleted && r.ParentId == parentId)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return nestedReplies;
        }

        public async Task<IEnumerable<TModel>> GetAllByPostIdAndUserIdAsync<TModel>(int postId, string userId)
        {
            var replies = await this.db.Replies
                .Where(r => r.PostId == postId &&
                            r.AuthorId == userId &&
                            !r.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return replies;
        }
    }
}