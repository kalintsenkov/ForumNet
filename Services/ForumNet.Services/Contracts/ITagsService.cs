namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagsService
    {
        Task CreateAsync(string name);

        Task DeleteAsync(int id);

        Task<bool> AreExisting(IEnumerable<int> ids);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId);
    }
}