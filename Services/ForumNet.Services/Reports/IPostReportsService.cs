namespace ForumNet.Services.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostReportsService
    {
        Task CreateAsync(string description, int postId, string authorId);

        Task<bool> DeleteAsync(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();
    }
}