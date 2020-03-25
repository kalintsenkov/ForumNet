namespace ForumNet.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Data;

    public class UsersService : IUsersService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMapper mapper;

        public UsersService(
            ForumDbContext db, 
            IDateTimeProvider dateTimeProvider, 
            IMapper mapper)
        {
            this.db = db;
            this.dateTimeProvider = dateTimeProvider;
            this.mapper = mapper;
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

        public async Task<bool> IsUsernameUsed(string username)
        {
            return await this.db.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<bool> IsUserDeleted(string username)
        {
            return await this.db.Users.AnyAsync(u => u.UserName == username && u.IsDeleted);
        }

        public async Task<TModel> GetByIdAsync<TModel>(string id)
        {
            var user = await this.db.Users
                .Where(u => u.Id == id && !u.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}