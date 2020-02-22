namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ITagsService
    {
        Task CreateAsync(string name);

        Task DeleteAsync(int id);
    }
}