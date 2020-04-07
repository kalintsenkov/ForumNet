namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task CreateAsync(string content, string authorId, string receiverId);

        Task<IEnumerable<TModel>> GetAllWithUserAsync<TModel>(string currentUserId, string userId);
    }
}
