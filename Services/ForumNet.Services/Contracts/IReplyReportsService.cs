namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReplyReportsService
    {
        Task CreateAsync(string description, int replyId, string authorId);

        Task<IEnumerable<TModel>> GetAll<TModel>();
    }
}