namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepliesService
    {
        Task CreateAsync(string description, int postId, string authorId);

        Task EditAsync(int id, string description);

        Task DeleteAsync(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId);
    }
}