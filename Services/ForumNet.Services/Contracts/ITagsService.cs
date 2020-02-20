namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ITagsService
    {
        Task Create(string name);

        Task Delete(int id);
    }
}