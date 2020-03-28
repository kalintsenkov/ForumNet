namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostReportsService
    {
        Task CreateAsync(string description, int postId, string authorId);

        Task<IEnumerable<TModel>> GetAll<TModel>();
    }
}