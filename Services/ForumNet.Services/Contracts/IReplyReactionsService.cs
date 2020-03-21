namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IReplyReactionsService
    {
        Task<int> ReactAsync(ReactionType reactionType, int replyId, string authorId);

        Task<int> GetLikesCountByReplyIdAsync(int replyId);

        Task<int> GetLovesCountByReplyIdAsync(int replyId);

        Task<int> GetWowCountByReplyIdAsync(int replyId);

        Task<int> GetHahaCountByReplyIdAsync(int replyId);

        Task<int> GetSadCountByReplyIdAsync(int replyId);

        Task<int> GetAngryCountByReplyIdAsync(int replyId);
    }
}