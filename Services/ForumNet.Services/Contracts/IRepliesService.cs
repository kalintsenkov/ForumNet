namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepliesService
    {
        Task CreateAsync(string description, int postId, string authorId);

        Task EditAsync(int id, string description);

        Task DeleteAsync(int id);

        Task<bool> IsExisting(int id);

        Task<string> GetAuthorIdById(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId, string sort);

        Task<IEnumerable<TModel>> GetAllByPostIdAndUserIdAsync<TModel>(int postId, string userId);
    }
}