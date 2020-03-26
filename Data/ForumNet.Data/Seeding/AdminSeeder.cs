namespace ForumNet.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using ForumNet.Common;
    using Microsoft.EntityFrameworkCore;
    using Models;

    internal class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ForumDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ForumUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<ForumRole>>();

            var isExisting = await userManager.Users.AnyAsync(u => u.UserName == GlobalConstants.AdministratorUserName);
            if (!isExisting)
            {
                var admin = new ForumUser
                {
                    UserName = GlobalConstants.AdministratorUserName,
                    Email = GlobalConstants.AdministratorEmail,
                    ProfilePicture = GlobalConstants.AdministratorProfilePicture,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, GlobalConstants.AdministratorPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                var isRoleExists = await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName);
                if (isRoleExists)
                {
                    await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}