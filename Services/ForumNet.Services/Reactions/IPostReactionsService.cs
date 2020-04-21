namespace ForumNet.Services.Reactions
{
    using System.Threading.Tasks;

    using Data.Models.Enums;

    public interface IPostReactionsService
    {
        Task<ReactionsCountServiceModel> ReactAsync(ReactionType reactionType, int postId, string authorId);

        Task<int> GetTotalCountAsync();
    }
}