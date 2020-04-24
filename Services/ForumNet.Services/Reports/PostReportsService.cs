namespace ForumNet.Services.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Providers.DateTime;

    public class PostReportsService : IPostReportsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public PostReportsService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
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

        public async Task<bool> DeleteAsync(int id)
        {
            var postReport = await this.db.PostReports.FirstOrDefaultAsync(pr => pr.Id == id && !pr.IsDeleted);
            if (postReport == null)
            {
                return false;
            }

            postReport.IsDeleted = true;
            postReport.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<TModel> GetByIdAsync<TModel>(int id) 
            => await this.db.PostReports
                .AsNoTracking()
                .Where(pr => pr.Id == id && !pr.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>() 
            => await this.db.PostReports
                .AsNoTracking()
                .Where(pr => !pr.IsDeleted && !pr.Post.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}