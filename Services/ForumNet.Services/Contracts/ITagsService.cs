namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagsService
    {
        Task CreateAsync(string name);

        Task DeleteAsync(int id);

        Task<bool> IsExistingAsync(int id);

        Task<bool> IsExistingAsync(string name);

        Task<bool> AreExistingAsync(IEnumerable<int> ids);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null);

        Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId);
    }
}