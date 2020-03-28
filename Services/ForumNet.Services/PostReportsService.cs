namespace ForumNet.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using Contracts;
    using Data;
    using Data.Models;

    public class PostReportsService : IPostReportsService
    {
        private readonly ForumDbContext db;
        private readonly IDateTimeProvider dateTimeProvider;

        public PostReportsService(ForumDbContext db, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string description, int postId, string authorId)
        {
            var postReport = new PostReport
            {
                Description = description,
                CreatedOn = this.dateTimeProvider.Now(),
                PostId = postId,
                AuthorId = authorId
            };

            await this.db.PostReports.AddAsync(postReport);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAll<TModel>()
        {
            throw new System.NotImplementedException();
        }
    }
}