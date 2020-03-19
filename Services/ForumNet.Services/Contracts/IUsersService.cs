namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task ModifyAsync(string id);

        Task DeleteAsync(string id);

        Task UndeleteAsync(string id);

        Task<int> LevelUpAsync(string id);

        Task<bool> IsUsernameUsed(string username);

        Task<bool> IsUserDeleted(string username);
    }
}