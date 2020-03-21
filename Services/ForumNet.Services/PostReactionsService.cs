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

        public async Task<int> ReactAsync(ReactionType reactionType, int postId, string authorId)
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

            return (int)reactionType;
        }

        public async Task<int> GetLikesCountByPostIdAsync(int postId)
        {
            var likes = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == ReactionType.Like);

            return likes;
        }

        public async Task<int> GetLovesCountByPostIdAsync(int postId)
        {
            var loves = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == ReactionType.Love);

            return loves;
        }

        public async Task<int> GetWowCountByPostIdAsync(int postId)
        {
            var wowCount = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == ReactionType.Wow);

            return wowCount;
        }

        public async Task<int> GetHahaCountByPostIdAsync(int postId)
        {
            var hahaCount = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == ReactionType.Haha);

            return hahaCount;
        }

        public async Task<int> GetSadCountByPostIdAsync(int postId)
        {
            var sadCount = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == ReactionType.Sad);

            return sadCount;
        }

        public async Task<int> GetAngryCountByPostIdAsync(int postId)
        {
            var angryCount = await this.db.PostReactions
                .Where(pr => !pr.Post.IsDeleted && pr.PostId == postId)
                .CountAsync(pr => pr.ReactionType == ReactionType.Angry);

            return angryCount;
        }
    }
}