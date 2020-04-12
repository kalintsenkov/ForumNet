namespace ForumNet.Services.Contracts
{
    using System.Threading.Tasks;

    using Data.Models.Enums;
    using Models;

    public interface IPostReactionsService
    {
        Task ReactAsync(ReactionType reactionType, int postId, string authorId);

        Task<int> GetTotalCountAsync();

        Task<ReactionsCountServiceModel> GetCountByPostIdAsync(int postId);
    }
}