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

    public class ChatService : IChatService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;

        public ChatService(ForumDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(string currentUserId)
        {
            var sentMessages = this.db.Messages
                .Where(m => !m.IsDeleted && 
                            (m.AuthorId == currentUserId || m.ReceiverId == currentUserId))
                .Select(m => m.Author)
                .OrderByDescending(m => m.CreatedOn);

            var receivedMessages = this.db.Messages
                .Where(m => !m.IsDeleted && 
                            (m.AuthorId == currentUserId || m.ReceiverId == currentUserId))
                .Select(m => m.Receiver)
                .OrderByDescending(m => m.CreatedOn);

            var concatenatedMessages = await sentMessages
                .Concat(receivedMessages)
                .Where(u => u.Id != currentUserId)
                .Distinct()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return concatenatedMessages;
        }
    }
}
