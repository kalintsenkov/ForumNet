namespace ForumNet.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatService
    {
        Task<IEnumerable<TModel>> GetAllConversationsAsync<TModel>(string currentUserId);
    }
}
