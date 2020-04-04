namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task CreateAsync(string content, string from, string to);

        Task<IEnumerable<TModel>> GetAllConversationsAsync<TModel>(string currentUserId);

        Task<IEnumerable<TModel>> GetAllWithUserAsync<TModel>(string currentUserId, string userId);
    }
}
