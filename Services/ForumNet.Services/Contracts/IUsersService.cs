namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task ModifyAsync(string id);

        Task DeleteAsync(string id);

        Task UndeleteAsync(string id);

        Task<int> LevelUpAsync(string id);

        Task<bool> FollowAsync(string userId, string followerId);

        Task<bool> IsUsernameUsed(string username);

        Task<bool> IsUserDeleted(string username);

        Task<bool> IsFollowedAlready(string id, string followerId);

        Task<int> GetFollowersCount(string id);

        Task<int> GetFollowingCount(string id);

        Task<TModel> GetByIdAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetFollowers<TModel>(string id);

        Task<IEnumerable<TModel>> GetFollowing<TModel>(string id);
    }
}