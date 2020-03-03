namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IPostsService
    {
        Task<int> CreateAsync(
            string title,
            PostType type,
            string description,
            string authorId,
            int categoryId,
            string imageOrVideoUrl = null);

        Task EditAsync(
            int id,
            string title,
            string description,
            int categoryId,
            IEnumerable<int> tagIds,
            string imageOrVideoUrl = null);

        Task DeleteAsync(int id);

        Task ViewAsync(int id);

        Task LikeAsync(int id);

        Task DislikeAsync(int id);

        Task AddTagsAsync(int id, IEnumerable<int> tagIds);

        Task<int> GetCountByUserId(string userId);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllWithDeletedAsync<TModel>();
    }
}