namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReplyReportsService
    {
        Task CreateAsync(string description, int replyId, string authorId);

        Task DeleteAsync(int id);

        Task<bool> IsExisting(int id);

        Task<TModel> GetById<TModel>(int id);

        Task<IEnumerable<TModel>> GetAll<TModel>();
    }
}