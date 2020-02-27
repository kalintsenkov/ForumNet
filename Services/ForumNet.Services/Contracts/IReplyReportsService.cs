namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IReplyReportsService
    {
        Task CreateAsync(string description, int replyId, string authorId);
    }
}