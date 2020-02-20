namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task Create(string name);

        Task Delete(int id);

        Task<TModel> GetById<TModel>(int id);
    }
}