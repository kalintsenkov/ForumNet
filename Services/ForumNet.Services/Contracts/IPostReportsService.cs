namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IPostReportsService
    {
        Task CreateAsync(string description, int postId, string authorId);
    }
}