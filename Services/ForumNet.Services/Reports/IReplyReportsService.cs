namespace ForumNet.Services.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReplyReportsService
    {
        Task CreateAsync(string description, int replyId, string authorId);

        Task<bool> DeleteAsync(int id);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>();
    }
}