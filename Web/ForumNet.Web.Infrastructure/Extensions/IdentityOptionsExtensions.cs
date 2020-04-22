namespace ForumNet.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;

    using Common;

    public static class IdentityOptionsExtensions
    {
        public static IdentityOptions SetIdentityOptions(this IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = GlobalConstants.UserPasswordMinLength;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;

            return options;
        }
    }
}
