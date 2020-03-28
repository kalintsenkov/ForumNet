namespace ForumNet.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Contracts;
    using Data;
    using Data.Models;

    public class PostReportsService : IPostReportsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public PostReportsService(
            ForumDbContext db,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider)
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

        public async Task DeleteAsync(int id)
        {
            var postReport = await this.db.PostReports.FirstOrDefaultAsync(pr => pr.Id == id && !pr.IsDeleted);

            postReport.IsDeleted = true;
            postReport.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExisting(int id)
        {
            return await this.db.PostReports.AnyAsync(pr => pr.Id == id && !pr.IsDeleted);
        }

        public async Task<TModel> GetById<TModel>(int id)
        {
            var postReport = await this.db.PostReports
                .Where(pr => pr.Id == id && !pr.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return postReport;
        }

        public async Task<IEnumerable<TModel>> GetAll<TModel>()
        {
            var reports = await this.db.PostReports
                .Where(pr => !pr.IsDeleted && !pr.Post.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return reports;
        }
    }
}