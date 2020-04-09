namespace ForumNet.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Contracts;
    using Data;
    using Data.Models;

    public class MessagesServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldAddOnlyOneMessageInDatabase()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var messagesService = new MessagesService(db, null, dateTimeProviderMock.Object);
            await messagesService.CreateAsync("Test", "123", "321");

            db.Messages.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateMethodShouldAddRightCategoryInDatabase()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var messagesService = new MessagesService(db, null, dateTimeProvider.Object);
            await messagesService.CreateAsync("Test", "123", "321");

            var expected = new Message
            {
                Id = 1,
                Content = "Test",
                AuthorId = "123",
                ReceiverId = "321",
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Messages.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        //[Fact]
        //public async Task GetAllWithUserMethodShouldReturnAllMessagesWithCorrectUserOrderedByCreationDate()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Message, Message>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    var messages = new List<Message>
        //    {
        //        new Message
        //        {
        //            Content = "Test message 1",
        //            AuthorId = "123",
        //            ReceiverId = "321",
        //            CreatedOn = dateTimeProvider.Object.Now()
        //        },
        //        new Message
        //        {
        //            Content = "Test message 2",
        //            AuthorId = "321",
        //            ReceiverId = "123",
        //            CreatedOn = dateTimeProvider.Object.Now()
        //        }
        //    };

        //    await db.Messages.AddRangeAsync(messages);
        //    await db.SaveChangesAsync();

        //    var messagesService = new MessagesService(db, mapper, dateTimeProvider.Object);
        //    var actual = await messagesService.GetAllWithUserAsync<Message>("123", "321");

        //    actual.Should().BeEquivalentTo(messages);
        //}
    }
}