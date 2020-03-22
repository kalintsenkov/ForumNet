namespace ForumNet.Services.Contracts
{
    using System;
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

        Task<bool> IsExisting(int id);

        Task<string> GetAuthorIdById(int id);

        Task<string> GetLatestActivityById(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string search = null);

        Task<IEnumerable<TModel>> GetAllPinnedAsync<TModel>();

        Task<IEnumerable<TModel>> GetAllByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllByCategoryIdAsync<TModel>(int categoryId, string search = null);

        Task<IEnumerable<TModel>> GetAllWithDeletedAsync<TModel>();
    }
}