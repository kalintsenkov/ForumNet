namespace ForumNet.Services.Tags
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

        Task<int> GetCountAsync(string searchFilter = null);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null, int skip = 0, int? take = null);

        Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId);
    }
}