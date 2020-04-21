﻿namespace ForumNet.Services.Messages
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

        public async Task CreateAsync(string content, string authorId, string receiverId)
        {
            var message = new Message
            {
                Content = content,
                AuthorId = authorId,
                ReceiverId = receiverId,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.Messages.AddAsync(message);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllWithUserAsync<TModel>(string currentUserId, string userId) 
            => await this.db.Messages
                .Where(m => !m.IsDeleted &&
                            ((m.ReceiverId == currentUserId && m.AuthorId == userId) ||
                             (m.ReceiverId == userId && m.AuthorId == currentUserId)))
                .OrderBy(m => m.CreatedOn)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
