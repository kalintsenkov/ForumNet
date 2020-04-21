namespace ForumNet.Services.Replies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Providers;
    using Users;

    public class RepliesService : IRepliesService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly IDateTimeProvider dateTimeProvider;

        public RepliesService(
            ForumDbContext db,
            IMapper mapper,
            IUsersService usersService,
            IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.usersService = usersService;
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

            await this.usersService.AddPointsAsync(authorId);
        }

        public async Task EditAsync(int id, string description)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            reply.Description = description;
            reply.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            reply.IsDeleted = true;
            reply.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
            await this.DeleteNestedAsync(id);
        }

        public async Task MakeBestAnswerAsync(int id)
        {
            var reply = await this.db.Replies.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            var bestAnswerReply = await this.db.Replies.FirstOrDefaultAsync(r => r.IsBestAnswer && !r.IsDeleted);
            if (bestAnswerReply == null)
            {
                reply.IsBestAnswer = true;

                await this.usersService.AddPointsAsync(reply.AuthorId, 5);
            }
            else
            {
                reply.IsBestAnswer = true;
                bestAnswerReply.IsBestAnswer = false;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExistingAsync(int id)
            => await this.db.Replies.AnyAsync(r => r.Id == id && !r.IsDeleted);

        public async Task<string> GetAuthorIdByIdAsync(int id)
            => await this.db.Replies
                .Where(r => r.Id == id && !r.IsDeleted)
                .Select(r => r.AuthorId)
                .FirstOrDefaultAsync();

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.db.Replies
                .Where(r => r.Id == id && !r.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId)
            => await this.db.Replies
                .Where(r => r.AuthorId == userId && !r.IsDeleted && !r.Post.IsDeleted)
                .OrderByDescending(p => p.CreatedOn)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId, string sort = null)
        {
            var queryable = this.db.Replies
                .Where(r => r.PostId == postId && !r.IsDeleted)
                .OrderByDescending(r => r.IsBestAnswer);

            queryable = sort switch
            {
                "recent" => queryable.ThenByDescending(r => r.CreatedOn),
                "most reacted" => queryable.ThenByDescending(r => r.Reactions.Count),
                "longest" => queryable.ThenByDescending(r => r.Description.Length),
                "shortest" => queryable.ThenBy(r => r.Description.Length),
                _ => queryable.ThenByDescending(r => r.CreatedOn)
            };

            var replies = await queryable
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return replies;
        }

        private async Task DeleteNestedAsync(int id)
        {
            var nestedReply = await this.db.Replies.FirstOrDefaultAsync(r => r.ParentId == id && !r.IsDeleted);
            if (nestedReply == null)
            {
                return;
            }

            nestedReply.IsDeleted = true;
            nestedReply.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
            await this.DeleteNestedAsync(nestedReply.Id);
        }
    }
}