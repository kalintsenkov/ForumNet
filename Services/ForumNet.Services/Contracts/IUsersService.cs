namespace ForumNet.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<string> GetId(ClaimsPrincipal claimsPrincipal);

        Task<int> LevelUp(string id);
    }
}