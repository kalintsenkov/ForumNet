namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task CreateAsync(string name);

        Task EditAsync(int id, string name);

        Task DeleteAsync(int id);

        Task<bool> IsExisting(int id);

        Task<bool> IsExisting(string name);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();
    }
}