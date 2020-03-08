namespace ForumNet.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using Contracts;
    using Data;
    using Data.Models;

    public class UsersService : IUsersService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly UserManager<ForumUser> userManager;

        public UsersService(ForumDbContext db, IDateTimeProvider dateTimeProvider, UserManager<ForumUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
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

        public async Task<string> GetIdAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            return user.Id;
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
    }
}