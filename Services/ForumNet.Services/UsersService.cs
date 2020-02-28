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
        private readonly UserManager<ForumUser> userManager;

        public UsersService(ForumDbContext db, UserManager<ForumUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<string> GetId(ClaimsPrincipal claimsPrincipal)
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            return user.Id;
        }

        public async Task<int> LevelUp(string id)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.Level++;

            await this.db.SaveChangesAsync();

            return user.Level;
        }
    }
}