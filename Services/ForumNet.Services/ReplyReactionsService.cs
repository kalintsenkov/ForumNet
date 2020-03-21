namespace ForumNet.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Data;
    using Data.Models;
    using Data.Models.Enums;

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

        public async Task<int> GetLikesCountByReplyIdAsync(int replyId)
        {
            var likes = await this.db.ReplyReactions
                .Where(rr => !rr.Reply.IsDeleted && rr.ReplyId == replyId)
                .CountAsync(rr => rr.ReactionType == ReactionType.Like);

            return likes;
        }

        public async Task<int> GetLovesCountByReplyIdAsync(int replyId)
        {
            var loves = await this.db.ReplyReactions
                .Where(rr => !rr.Reply.IsDeleted && rr.ReplyId == replyId)
                .CountAsync(rr => rr.ReactionType == ReactionType.Love);

            return loves;
        }

        public async Task<int> GetWowCountByReplyIdAsync(int replyId)
        {
            var wowCount = await this.db.ReplyReactions
                .Where(rr => !rr.Reply.IsDeleted && rr.ReplyId == replyId)
                .CountAsync(rr => rr.ReactionType == ReactionType.Wow);

            return wowCount;
        }

        public async Task<int> GetHahaCountByReplyIdAsync(int replyId)
        {
            var hahaCount = await this.db.ReplyReactions
                .Where(rr => !rr.Reply.IsDeleted && rr.ReplyId == replyId)
                .CountAsync(rr => rr.ReactionType == ReactionType.Haha);

            return hahaCount;
        }

        public async Task<int> GetSadCountByReplyIdAsync(int replyId)
        {
            var sadCount = await this.db.ReplyReactions
                .Where(rr => !rr.Reply.IsDeleted && rr.ReplyId == replyId)
                .CountAsync(rr => rr.ReactionType == ReactionType.Sad);

            return sadCount;
        }

        public async Task<int> GetAngryCountByReplyIdAsync(int replyId)
        {
            var angryCount = await this.db.ReplyReactions
                .Where(rr => !rr.Reply.IsDeleted && rr.ReplyId == replyId)
                .CountAsync(rr => rr.ReactionType == ReactionType.Angry);

            return angryCount;
        }
    }
}