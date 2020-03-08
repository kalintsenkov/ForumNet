namespace ForumNet.Services
{
    using System.Threading.Tasks;

    using Contracts;
    using Data;
    using Data.Models;

    public class ReplyReportsService : IReplyReportsService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;

        public ReplyReportsService(ForumDbContext db, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string description, int replyId, string authorId)
        {
            var replyReport = new ReplyReport
            {
                Description = description,
                CreatedOn = this.dateTimeProvider.Now(),
                ReplyId = replyId,
                AuthorId = authorId
            };

            await this.db.ReplyReports.AddAsync(replyReport);
            await this.db.SaveChangesAsync();
        }
    }
}