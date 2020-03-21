namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IPostReactionsService
    {
        Task<int> ReactAsync(ReactionType reactionType, int postId, string authorId);

        Task<int> GetLikesCountByPostIdAsync(int postId);

        Task<int> GetLovesCountByPostIdAsync(int postId);

        Task<int> GetWowCountByPostIdAsync(int postId);

        Task<int> GetHahaCountByPostIdAsync(int postId);

        Task<int> GetSadCountByPostIdAsync(int postId);

        Task<int> GetAngryCountByPostIdAsync(int postId);
    }
}