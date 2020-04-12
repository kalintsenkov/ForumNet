namespace ForumNet.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Models;

    public class ReplyReactionsService : IReplyReactionsService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;

        public ReplyReactionsService(ForumDbContext db, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task ReactAsync(ReactionType reactionType, int replyId, string authorId)
        {
            var replyReaction = await this.db.ReplyReactions
                .FirstOrDefaultAsync(rr => rr.ReplyId == replyId && rr.AuthorId == authorId);

            if (replyReaction == null)
            {
                replyReaction = new ReplyReaction
                {
                    ReactionType = reactionType,
                    ReplyId = replyId,
                    AuthorId = authorId,
                    CreatedOn = this.dateTimeProvider.Now()
                };

                await this.db.ReplyReactions.AddAsync(replyReaction);
            }
            else
            {
                replyReaction.ReactionType = replyReaction.ReactionType == reactionType ? ReactionType.Neutral : reactionType;
                replyReaction.ModifiedOn = this.dateTimeProvider.Now();
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await this.db.ReplyReactions.Where(pr => !pr.Reply.IsDeleted).CountAsync();
        }

        public async Task<ReactionsCountServiceModel> GetCountByReplyIdAsync(int replyId)
        {
            var reactionsCount = new ReactionsCountServiceModel
            {
                Likes = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Like, replyId),
                Loves = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Love, replyId),
                HahaCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Haha, replyId),
                WowCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Wow, replyId),
                SadCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Sad, replyId),
                AngryCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Angry, replyId)
            };

            return reactionsCount;
        }

        private async Task<int> GetCountByTypeAndReplyIdAsync(ReactionType reactionType, int replyId)
        {
            var count = await this.db.ReplyReactions
                .Where(pr => !pr.Reply.IsDeleted && pr.ReplyId == replyId)
                .CountAsync(pr => pr.ReactionType == reactionType);

            return count;
        }
    }
}