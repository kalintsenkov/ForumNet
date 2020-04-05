namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IPostReactionsService
    {
        Task ReactAsync(ReactionType reactionType, int postId, string authorId);

        Task<int> GetTotalCountAsync();

        Task<(int Likes, int Loves, int Haha, int Wow, int Sad, int Angry)> GetCountByPostIdAsync(int postId);
    }
}