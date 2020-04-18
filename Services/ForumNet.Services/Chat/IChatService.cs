namespace ForumNet.Services.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatService
    {
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(string currentUserId);
    }
}
