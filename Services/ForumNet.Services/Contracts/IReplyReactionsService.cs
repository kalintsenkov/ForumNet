namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    using Data.Models.Enums;
    using Models;

    public interface IReplyReactionsService
    {
        Task ReactAsync(ReactionType reactionType, int replyId, string authorId);

        Task<int> GetTotalCountAsync();

        Task<ReactionsCountServiceModel> GetCountByReplyIdAsync(int replyId);
    }
}