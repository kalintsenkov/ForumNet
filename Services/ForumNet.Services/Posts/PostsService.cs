namespace ForumNet.Services.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Providers.DateTime;
    using Users;

    public class PostsService : IPostsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly IDateTimeProvider dateTimeProvider;

        public PostsService(
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

        public async Task<int> CreateAsync(
            string title, 
            PostType type, 
            string description, 
            string authorId, 
            int categoryId, 
            IEnumerable<int> tagIds)
        {
            var post = new Post
            {
                Title = title,
                Type = type,
                Description = description,
                CreatedOn = this.dateTimeProvider.Now(),
                AuthorId = authorId,
                CategoryId = categoryId
            };

            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();
            await this.AddTagsAsync(post.Id, tagIds);

            await this.usersService.AddPointsAsync(authorId);

            return post.Id;
        }

        public async Task EditAsync(
            int id, 
            string title, 
            string description, 
            int categoryId, 
            IEnumerable<int> tagIds)
        {
            var post = await this.GetByIdAsync(id);

            await this.RemoveTagsAsync(id, post);

            post.Title = title;
            post.Description = description;
            post.CategoryId = categoryId;
            post.ModifiedOn = this.dateTimeProvider.Now();

            await this.AddTagsAsync(id, tagIds);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post = await this.GetByIdAsync(id);

            post.IsDeleted = true;
            post.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task ViewAsync(int id)
        {
            var post = await this.GetByIdAsync(id);

            post.Views++;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> PinAsync(int id)
        {
            var post = await this.GetByIdAsync(id);

            post.IsPinned = !post.IsPinned;
            post.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();

            return post.IsPinned;
        }

        public async Task<bool> IsExistingAsync(int id)
            => await this.db.Posts.AnyAsync(p => p.Id == id && !p.IsDeleted);

        public async Task<int> GetCountAsync()
            => await this.db.Posts
                .Where(p => !p.IsDeleted)
                .CountAsync();

        public async Task<int> GetFollowingCountAsync(string userId)
            => await this.db.Posts
                .Where(p => !p.IsDeleted && p.Author.Followers
                    .Select(f => f.FollowerId)
                    .FirstOrDefault() == userId)
                .CountAsync();

        public async Task<string> GetAuthorIdByIdAsync(int id)
            => await this.db.Posts
                .Where(p => p.Id == id && !p.IsDeleted)
                .Select(p => p.AuthorId)
                .FirstOrDefaultAsync();

        public async Task<string> GetLatestActivityByIdAsync(int id)
        {
            var latestPostReply = await this.db.Posts
                .Where(p => p.Id == id && !p.IsDeleted)
                .SelectMany(p => p.Replies)
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.CreatedOn)
                .FirstOrDefaultAsync(r => r.PostId == id);

            var currentTime = this.dateTimeProvider.Now();

            if (latestPostReply != null)
            {
                var latestPostReplyActivity = this.CalculateLatestActivity(currentTime, latestPostReply.CreatedOn);
                return latestPostReplyActivity;
            }

            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            var postLatestActivity = this.CalculateLatestActivity(currentTime, post.CreatedOn);

            return postLatestActivity;
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id)
            => await this.db.Posts
                .AsNoTracking()
                .Where(p => p.Id == id && !p.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetSuggestedAsync<TModel>(int take)
            => await this.db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.CreatedOn)
                .Where(p => !p.IsDeleted)
                .Take(take)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null, int skip = 0, int? take = null)
        {
            var queryable = this.db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.Reactions
                    .Count(r => r.ReactionType != ReactionType.Neutral))
                .ThenByDescending(p => p.CreatedOn)
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(p => p.Title.Contains(search));
            }

            queryable = take.HasValue
                ? queryable.Skip(skip).Take(take.Value)
                : queryable.Skip(skip);

            var posts = await queryable
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        public async Task<IEnumerable<TModel>> GetAllByTagIdAsync<TModel>(int tagId, string search = null)
        {
            var queryable = this.db.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.Tags
                    .Select(t => t.TagId).Contains(tagId));

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(p => p.Title.Contains(search));
            }

            var posts = await queryable
                .OrderByDescending(p => p.CreatedOn)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        public async Task<IEnumerable<TModel>> GetAllByCategoryIdAsync<TModel>(int categoryId, string search = null)
        {
            var queryable = this.db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(p => p.Title.Contains(search));
            }

            var posts = await queryable
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        public async Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId)
            => await this.db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.AuthorId == userId && !p.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<TModel>> GetAllFollowingByUserIdAsync<TModel>(
            string userId, 
            string search = null, 
            int skip = 0, 
            int? take = null)
        {
            var queryable = this.db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.Author.Followers
                    .Where(x => !x.IsDeleted && x.FollowerId == userId)
                    .Select(x => x.FollowerId)
                    .FirstOrDefault() == userId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable.Where(p => p.Title.Contains(search));
            }

            queryable = take.HasValue
                ? queryable.Skip(skip).Take(take.Value)
                : queryable.Skip(skip);

            var posts = await queryable
                .Where(p => !p.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        private async Task<Post> GetByIdAsync(int id)
            => await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        private async Task AddTagsAsync(int id, IEnumerable<int> tagIds)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            foreach (var tagId in tagIds)
            {
                post.Tags.Add(new PostTag
                {
                    PostId = id,
                    TagId = tagId
                });
            }

            await this.db.SaveChangesAsync();
        }

        private async Task RemoveTagsAsync(int id, Post post)
        {
            var postTags = await this.db.PostsTags.Where(pt => pt.PostId == id).ToListAsync();
            foreach (var tag in postTags)
            {
                post.Tags.Remove(tag);
            }

            await this.db.SaveChangesAsync();
        }

        private string CalculateLatestActivity(DateTime currentTime, DateTime latestPostTime)
        {
            const decimal totalDays = 365.25m;
            const char yearsPostfix = 'y';
            const char daysPostfix = 'd';
            const char hoursPostfix = 'h';
            const char minutesPostfix = 'm';

            var activity = currentTime - latestPostTime;
            var daysFromNow = activity.Days;
            var hoursFromNow = activity.Hours;
            var minutesFromNow = activity.Minutes;
            var years = (int)(daysFromNow / totalDays);

            var result = $"{years}{yearsPostfix}";

            if (years > 0)
            {
                return result;
            }

            if (daysFromNow != 0)
            {
                result = $"{daysFromNow}{daysPostfix}";
            }
            else
            {
                result = hoursFromNow == 0 ? $"{minutesFromNow}{minutesPostfix}" : $"{hoursFromNow}{hoursPostfix}";
            }

            return result;
        }
    }
}