namespace ForumNet.Services.Reactions
{
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IReplyReactionsService
    {
        Task ReactAsync(ReactionType reactionType, int replyId, string authorId);

        Task<int> GetTotalCountAsync();

        Task<ReactionsCountServiceModel> GetCountByReplyIdAsync(int replyId);
    }
}