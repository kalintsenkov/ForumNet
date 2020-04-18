namespace ForumNet.Services.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Common;
    using Data;
    using Data.Models;

    public class UsersService : IUsersService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public UsersService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task ModifyAsync(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            user.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            user.Email = null;
            user.NormalizedEmail = null;
            user.IsDeleted = true;
            user.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<int> AddPointsAsync(string id, int points = 1)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            user.Points += points;

            await this.db.SaveChangesAsync();

            return user.Points;
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

                isFollowed = true;
                await this.db.UsersFollowers.AddAsync(userFollower);
            }
            else
            {
                if (userFollower.IsDeleted)
                {
                    isFollowed = true;
                    userFollower.IsDeleted = false;
                    userFollower.DeletedOn = null;
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

        public async Task<bool> IsUsernameUsedAsync(string username) 
            => await this.db.Users.AnyAsync(u => u.UserName == username && !u.IsDeleted);

        public async Task<bool> IsDeletedAsync(string username) 
            => await this.db.Users.AnyAsync(u => u.UserName == username && u.IsDeleted);

        public async Task<bool> IsFollowedAlreadyAsync(string id, string followerId) 
            => await this.db.UsersFollowers
                .AnyAsync(uf => uf.UserId == id && 
                                uf.FollowerId == followerId && 
                                !uf.IsDeleted);

        public async Task<int> GetTotalCountAsync() 
            => await this.db.Users
                .Where(u => !u.IsDeleted)
                .CountAsync();

        public async Task<int> GetFollowersCountAsync(string id) 
            => await this.db.UsersFollowers
                .Where(uf => !uf.IsDeleted && 
                             !uf.User.IsDeleted && 
                             !uf.Follower.IsDeleted && 
                             uf.UserId == id)
                .CountAsync();

        public async Task<int> GetFollowingCountAsync(string id) 
            => await this.db.UsersFollowers
                .Where(uf => !uf.IsDeleted && 
                             !uf.User.IsDeleted && 
                             !uf.Follower.IsDeleted && 
                             uf.FollowerId == id)
                .CountAsync();

        public async Task<TModel> GetByIdAsync<TModel>(string id) 
            => await this.db.Users
                .AsNoTracking()
                .Where(u => u.Id == id && !u.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>() 
            => await this.db.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<TModel>> GetAdminsAsync<TModel>()
        {
            var adminRoleId = await this.db.Roles
                .Where(r => r.Name == GlobalConstants.AdministratorRoleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var admins = await this.db.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.Roles
                    .Select(r => r.RoleId)
                    .FirstOrDefault() == adminRoleId)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return admins;
        }

        public async Task<IEnumerable<TModel>> GetFollowersAsync<TModel>(string id) 
            => await this.db.UsersFollowers
                .AsNoTracking()
                .Where(uf => uf.UserId == id && 
                             !uf.IsDeleted && 
                             !uf.Follower.IsDeleted)
                .Select(uf => uf.Follower)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<TModel>> GetFollowingAsync<TModel>(string id) 
            => await this.db.UsersFollowers
                .AsNoTracking()
                .Where(uf => uf.FollowerId == id && 
                             !uf.IsDeleted && 
                             !uf.User.IsDeleted)
                .Select(uf => uf.User)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}