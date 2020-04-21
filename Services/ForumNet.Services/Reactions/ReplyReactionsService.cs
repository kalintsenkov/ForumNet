namespace ForumNet.Services.Reactions
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Providers;

    public class ReplyReactionsService : IReplyReactionsService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;

        public ReplyReactionsService(ForumDbContext db, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<ReactionsCountServiceModel> ReactAsync(ReactionType reactionType, int replyId, string authorId)
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
                replyReaction.ModifiedOn = this.dateTimeProvider.Now();
                replyReaction.ReactionType = replyReaction.ReactionType == reactionType 
                    ? ReactionType.Neutral 
                    : reactionType;
            }

            await this.db.SaveChangesAsync();

            return await this.GetCountByReplyIdAsync(replyId);
        }

        public async Task<int> GetTotalCountAsync() 
            => await this.db.ReplyReactions
                .Where(pr => !pr.Reply.IsDeleted)
                .CountAsync();

        private async Task<ReactionsCountServiceModel> GetCountByReplyIdAsync(int replyId) 
            => new ReactionsCountServiceModel
            {
                Likes = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Like, replyId),
                Loves = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Love, replyId),
                HahaCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Haha, replyId),
                WowCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Wow, replyId),
                SadCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Sad, replyId),
                AngryCount = await this.GetCountByTypeAndReplyIdAsync(ReactionType.Angry, replyId)
            };

        private async Task<int> GetCountByTypeAndReplyIdAsync(ReactionType reactionType, int replyId) 
            => await this.db.ReplyReactions
                .Where(pr => !pr.Reply.IsDeleted && pr.ReplyId == replyId)
                .CountAsync(pr => pr.ReactionType == reactionType);
    }
}