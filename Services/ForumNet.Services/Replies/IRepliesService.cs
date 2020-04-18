namespace ForumNet.Services.Replies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepliesService
    {
        Task CreateAsync(string description, int? parentId, int postId, string authorId);

        Task EditAsync(int id, string description);

        Task DeleteAsync(int id);

        Task MakeBestAnswerAsync(int id);

        Task<bool> IsExistingAsync(int id);

        Task<string> GetAuthorIdByIdAsync(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllByPostIdAsync<TModel>(int postId, string sort = null);
    }
}