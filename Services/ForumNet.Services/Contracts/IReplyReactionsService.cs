namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IReplyReactionsService
    {
        Task ReactAsync(ReactionType reactionType, int replyId, string authorId);

        Task<int> GetTotalCount();

        Task<(int Likes, int Loves, int Haha, int Wow, int Sad, int Angry)> GetCountByReplyIdAsync(int replyId);
    }
}