namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task Add(string name);
    }
}