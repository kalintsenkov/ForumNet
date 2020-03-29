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
    using ForumNet.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public UsersService(
            ForumDbContext db,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task ModifyAsync(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.IsDeleted = true;
            user.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task UndeleteAsync(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.IsDeleted = false;
            user.DeletedOn = null;

            await this.db.SaveChangesAsync();
        }

        public async Task<int> LevelUpAsync(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.Level++;

            await this.db.SaveChangesAsync();

            return user.Level;
        }

        public async Task<bool> FollowAsync(string userId, string followerId)
        {
            var isFollowed = false;
            var userFollower = await this.db.UsersFollowers
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FollowerId == followerId);

            if (userFollower == null)
            {
                userFollower = new UserFollower
                {
                    UserId = userId,
                    FollowerId = followerId,
                    CreatedOn = this.dateTimeProvider.Now()
                };

                await this.db.UsersFollowers.AddAsync(userFollower);
                isFollowed = true;
            }
            else
            {
                if (userFollower.IsDeleted)
                {
                    userFollower.IsDeleted = false;
                    userFollower.DeletedOn = null;
                    isFollowed = true;
                    userFollower.CreatedOn = this.dateTimeProvider.Now();
                    userFollower.ModifiedOn = this.dateTimeProvider.Now();
                }
                else
                {
                    userFollower.IsDeleted = true;
                    userFollower.DeletedOn = this.dateTimeProvider.Now();
                }
            }

            await this.db.SaveChangesAsync();

            return isFollowed;
        }

        public async Task<bool> IsUsernameUsed(string username)
        {
            return await this.db.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<bool> IsUserDeleted(string username)
        {
            return await this.db.Users.AnyAsync(u => u.UserName == username && u.IsDeleted);
        }

        public async Task<int> GetFollowersCount(string id)
        {
            return await this.db.UsersFollowers.CountAsync(u => !u.IsDeleted && u.UserId == id);
        }

        public async Task<int> GetFollowingCount(string id)
        {
            return await this.db.UsersFollowers.CountAsync(u => !u.IsDeleted && u.FollowerId == id);
        }

        public async Task<TModel> GetByIdAsync<TModel>(string id)
        {
            var user = await this.db.Users
                .Where(u => u.Id == id && !u.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<TModel>> GetFollowers<TModel>(string id)
        {
            var followers = await this.db.UsersFollowers
                .Where(uf => uf.UserId == id && !uf.IsDeleted)
                .Select(uf => uf.Follower)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return followers;
        }

        public async Task<IEnumerable<TModel>> GetFollowing<TModel>(string id)
        {
            var following = await this.db.UsersFollowers
                .Where(uf => uf.FollowerId == id && !uf.IsDeleted)
                .Select(uf => uf.User)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return following;
        }
    }
}