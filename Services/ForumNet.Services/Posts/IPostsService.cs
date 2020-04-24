namespace ForumNet.Services.Posts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IPostsService
    {
        Task<int> CreateAsync(string title, PostType type, string description, string authorId, int categoryId, IEnumerable<int> tagIds);

        Task EditAsync(int id, string title, string description, int categoryId, IEnumerable<int> tagIds);

        Task DeleteAsync(int id);

        Task ViewAsync(int id);

        Task<bool> PinAsync(int id);

        Task<bool> IsExistingAsync(int id);

        Task<int> GetCountAsync();

        Task<int> GetFollowingCountAsync(string userId);

        Task<string> GetAuthorIdByIdAsync(int id);

        Task<string> GetLatestActivityByIdAsync(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetSuggestedAsync<TModel>(int take);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null, int skip = 0, int? take = null);

        Task<IEnumerable<TModel>> GetAllByTagIdAsync<TModel>(int tagId, string search = null);

        Task<IEnumerable<TModel>> GetAllByCategoryIdAsync<TModel>(int categoryId, string search = null);

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllFollowingByUserIdAsync<TModel>(string userId, string search = null, int skip = 0, int? take = null);
    }
}