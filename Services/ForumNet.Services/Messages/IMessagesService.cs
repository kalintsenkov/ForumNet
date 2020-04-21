namespace ForumNet.Services.Messages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task CreateAsync(string content, string authorId, string receiverId);

        Task<string> GetLastActivityAsync(string currentUserId, string userId);

        Task<string> GetLastMessageAsync(string currentUserId, string userId);

        Task<IEnumerable<TModel>> GetAllWithUserAsync<TModel>(string currentUserId, string userId);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string currentUserId);
    }
}
