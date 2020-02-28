namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<bool> CreateAsync(
            string title,
            string postType,
            string description,
            string authorId,
            int categoryId,
            string imageOrVideoUrl = null);

        Task ViewAsync(int id);

        Task LikeAsync(int id);

        Task DislikeAsync(int id);

        Task EditAsync(int id, string title, string description, int categoryId);

        Task DeleteAsync(int id);

        Task AddTagsAsync(int id, IEnumerable<int> tagIds);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllWithDeletedAsync<TModel>();
    }
}