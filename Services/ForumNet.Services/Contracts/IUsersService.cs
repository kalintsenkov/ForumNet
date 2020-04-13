namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task ModifyAsync(string id);

        Task DeleteAsync(string id);

        Task<int> AddPointsAsync(string id, int points = 1);

        Task<bool> FollowAsync(string userId, string followerId);

        Task<bool> IsUsernameUsedAsync(string username);

        Task<bool> IsDeletedAsync(string username);

        Task<bool> IsFollowedAlreadyAsync(string id, string followerId);

        Task<int> GetTotalCountAsync();

        Task<int> GetFollowersCountAsync(string id);

        Task<int> GetFollowingCountAsync(string id);

        Task<TModel> GetByIdAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        Task<IEnumerable<TModel>> GetAdminsAsync<TModel>();

        Task<IEnumerable<TModel>> GetFollowersAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetFollowingAsync<TModel>(string id);
    }
}