namespace ForumNet.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Data;
    using Data.Models;
    using Data.Models.Enums;

    public class PostReactionsService: IPostReactionsService
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
                postReaction.ReactionType = postReaction.ReactionType == reactionType ? ReactionType.Neutral : reactionType;
                postReaction.ModifiedOn = this.dateTimeProvider.Now();
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {

            return await this.db.PostReactions.Where(pr => !pr.Post.IsDeleted).CountAsync();
        }

        public async Task<(int Likes, int Loves, int Haha, int Wow, int Sad, int Angry)> GetCountByPostIdAsync(int postId)
        {
            var likes = await this.GetCountByTypeAndPostIdAsync(ReactionType.Like, postId);
            var loves = await this.GetCountByTypeAndPostIdAsync(ReactionType.Love, postId);
            var haha = await this.GetCountByTypeAndPostIdAsync(ReactionType.Haha, postId);
            var wow = await this.GetCountByTypeAndPostIdAsync(ReactionType.Wow, postId);
            var sad = await this.GetCountByTypeAndPostIdAsync(ReactionType.Sad, postId);
            var angry = await this.GetCountByTypeAndPostIdAsync(ReactionType.Angry, postId);

            return (likes, loves, haha, wow, sad, angry);
        }

        private async Task<int> GetCountByTypeAndPostIdAsync(ReactionType reactionType, int postId)
        {
            var count = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == reactionType);

            return count;
        }
    }
}