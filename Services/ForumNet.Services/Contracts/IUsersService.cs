namespace ForumNet.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<string> GetIdAsync(ClaimsPrincipal claimsPrincipal);

        Task<int> LevelUpAsync(string id);

        Task DeleteAsync(string id);
    }
}