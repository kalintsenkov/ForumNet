namespace ForumNet.Services.Reactions
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Data.Models.Enums;

    public class PostReactionsService : IPostReactionsService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;

        public PostReactionsService(ForumDbContext db, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task ReactAsync(ReactionType reactionType, int postId, string authorId)
        {
            var postReaction = await this.db.PostReactions
                .FirstOrDefaultAsync(pr => pr.PostId == postId && pr.AuthorId == authorId);

            if (postReaction == null)
            {
                postReaction = new PostReaction
                {
                    ReactionType = reactionType,
                    PostId = postId,
                    AuthorId = authorId,
                    CreatedOn = this.dateTimeProvider.Now()
                };

                await this.db.PostReactions.AddAsync(postReaction);
            }
            else
            {
                postReaction.ModifiedOn = this.dateTimeProvider.Now();
                postReaction.ReactionType = postReaction.ReactionType == reactionType
                    ? ReactionType.Neutral 
                    : reactionType;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetTotalCountAsync() 
            => await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted)
                .CountAsync();

        public async Task<ReactionsCountServiceModel> GetCountByPostIdAsync(int postId)
            => new ReactionsCountServiceModel
            {
                Likes = await this.GetCountByTypeAndPostIdAsync(ReactionType.Like, postId),
                Loves = await this.GetCountByTypeAndPostIdAsync(ReactionType.Love, postId),
                HahaCount = await this.GetCountByTypeAndPostIdAsync(ReactionType.Haha, postId),
                WowCount = await this.GetCountByTypeAndPostIdAsync(ReactionType.Wow, postId),
                SadCount = await this.GetCountByTypeAndPostIdAsync(ReactionType.Sad, postId),
                AngryCount = await this.GetCountByTypeAndPostIdAsync(ReactionType.Angry, postId)
            };

        private async Task<int> GetCountByTypeAndPostIdAsync(ReactionType reactionType, int postId) 
            => await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == reactionType);
    }
}