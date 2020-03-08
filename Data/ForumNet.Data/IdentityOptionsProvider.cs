namespace ForumNet.Data
{
    using Microsoft.AspNetCore.Identity;

    using Common;

    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = DataConstants.UserPasswordMinLength;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
        }
    }
}
