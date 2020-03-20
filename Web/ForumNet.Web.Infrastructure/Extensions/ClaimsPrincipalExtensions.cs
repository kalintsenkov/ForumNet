namespace ForumNet.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    using Common;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static bool IsAdministrator(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.IsInRole(GlobalConstants.AdministratorRoleName);
    }
}