namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        // TODO: Add tags -> IEnumerable<int> tagIds
        Task Create(string title, string description, string authorId, int categoryId);

        Task View(int id);

        Task Like(int id);

        Task Dislike(int id);

        Task Edit(int id, string title, string description, int categoryId);

        Task Delete(int id);

        Task<IEnumerable<TModel>> GetAll<TModel>();
    }
}