namespace ForumNet.Services.Tests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Data;
    using Data.Models;
    using Messages;
    using Providers;

    public class MessagesServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldAddMessageInDatabase()
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
        public async Task CreateMethodShouldAddRightMessageInDatabase()
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
    }
}