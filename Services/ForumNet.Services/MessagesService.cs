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

    public class MessagesService : IMessagesService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public MessagesService(ForumDbContext db, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task CreateAsync(string content, string from, string to)
        {
            var message = new Message
            {
                Content = content,
                AuthorId = from,
                ReceiverId = to,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.Messages.AddAsync(message);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllConversationsAsync<TModel>(string currentUserId)
        {
            var sentMessages = this.db.Messages
                .Where(m => !m.IsDeleted && m.AuthorId != currentUserId)
                .Select(m => m.Author)
                .OrderByDescending(m => m.CreatedOn);

            var receivedMessages = this.db.Messages
                .Where(m => !m.IsDeleted && m.ReceiverId != currentUserId)
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

        public async Task<IEnumerable<TModel>> GetAllWithUserAsync<TModel>(string currentUserId, string userId)
        {
            var messages = await this.db.Messages
                .Where(m => (m.ReceiverId == currentUserId && m.AuthorId == userId)
                        || (m.ReceiverId == userId && m.AuthorId == currentUserId))
                .OrderBy(m => m.CreatedOn)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return messages;
        }
    }
}
