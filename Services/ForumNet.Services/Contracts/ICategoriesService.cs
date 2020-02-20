namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task Create(string name);

        Task Delete(int id);

        Task<TModel> GetById<TModel>(int id);

        Task<IEnumerable<TModel>> GetAll<TModel>();
    }
}