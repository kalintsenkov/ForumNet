namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IPostsService
    {
        Task<int> CreateAsync(string title, PostType type, string description, string authorId, int categoryId);

        Task EditAsync(int id, string title, string description, int categoryId, IEnumerable<int> tagIds);

        Task DeleteAsync(int id);

        Task ViewAsync(int id);

        Task<bool> PinAsync(int id);

        Task AddTagsAsync(int id, IEnumerable<int> tagIds);

        Task<bool> IsExistingAsync(int id);

        Task<int> GetCountAsync();

        Task<int> GetFollowingCountAsync(string userId);

        Task<string> GetAuthorIdByIdAsync(int id);

        Task<string> GetLatestActivityByIdAsync(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetSuggestedAsync<TModel>(int take);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(int skip = 0, int take = 0, string search = null);

        Task<IEnumerable<TModel>> GetAllByTagIdAsync<TModel>(int tagId, string search = null);

        Task<IEnumerable<TModel>> GetAllByCategoryIdAsync<TModel>(int categoryId, string search = null);

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllFollowingByUserIdAsync<TModel>(string userId, int skip = 0, int take = 0, string search = null);
    }
}