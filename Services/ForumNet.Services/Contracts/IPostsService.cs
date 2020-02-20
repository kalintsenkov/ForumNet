namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task Create(string title, string description, string authorId, int categoryId);

        Task View(int id);

        Task Like(int id);

        Task Dislike(int id);
    }
}