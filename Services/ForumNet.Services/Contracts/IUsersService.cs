namespace ForumNet.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task ModifyAsync(string id);

        Task DeleteAsync(string id);

        Task UndeleteAsync(string id);

        Task<string> GetIdAsync(ClaimsPrincipal claimsPrincipal);

        Task<int> LevelUpAsync(string id);

        Task<bool> IsUsernameUsed(string username);

        Task<bool> IsUserDeleted(string username);
    }
}