namespace ForumNet.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    public static class CookiePolicyOptionsExtensions
    {
        public static CookiePolicyOptions SetCookiePolicyOptions(this CookiePolicyOptions options)
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;

            return options;
        }
    }
}
