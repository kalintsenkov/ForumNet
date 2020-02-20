namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IPostReportsService
    {
        Task Create(string description, int postId, string authorId);
    }
}