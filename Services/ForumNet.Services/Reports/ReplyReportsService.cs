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
    using Providers;

    public class ReplyReportsService : IReplyReportsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public ReplyReportsService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string description, int replyId, string authorId)
        {
            var replyReport = new ReplyReport
            {
                Description = description,
                ReplyId = replyId,
                AuthorId = authorId,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.ReplyReports.AddAsync(replyReport);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var replyReport = await this.db.ReplyReports.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            replyReport.IsDeleted = true;
            replyReport.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsExistingAsync(int id) 
            => await this.db.ReplyReports.AnyAsync(r => r.Id == id && !r.IsDeleted);

        public async Task<TModel> GetByIdAsync<TModel>(int id) 
            => await this.db.ReplyReports
                .Where(r => r.Id == id && !r.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>() 
            => await this.db.ReplyReports
                .Where(r => !r.IsDeleted && !r.Reply.IsDeleted)
                .AsNoTracking()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}