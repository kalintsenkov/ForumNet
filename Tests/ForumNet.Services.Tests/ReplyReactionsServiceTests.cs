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
    using Data.Models.Enums;
    using Providers.DateTime;
    using Reactions;

    public class ReplyReactionsServiceTests
    {
        [Theory]
        [InlineData("Test 1", ReactionType.Like)]
        [InlineData("Test 2", ReactionType.Love)]
        [InlineData("Test 3", ReactionType.Haha)]
        [InlineData("Test 4", ReactionType.Wow)]
        [InlineData("Test 5", ReactionType.Sad)]
        [InlineData("Test 6", ReactionType.Angry)]
        public async Task ReactMethodShouldAddReactionInDatabaseIfNotExistsAlready(string description, ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var reply = new Reply
            {
                Id = 1,
                Description = description,
                AuthorId = guid,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Replies.AddAsync(reply);
            await db.SaveChangesAsync();

            var replyReactionsService = new ReplyReactionsService(db, dateTimeProvider.Object);
            var result = await replyReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.ReplyReactions.FirstOrDefaultAsync();
            var expected = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                Reply = reply,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionsCountServiceModel>();
        }

        [Theory]
        [InlineData(ReactionType.Love)]
        [InlineData(ReactionType.Haha)]
        [InlineData(ReactionType.Wow)]
        [InlineData(ReactionType.Sad)]
        [InlineData(ReactionType.Angry)]
        public async Task ReactMethodShouldChangeReactionIfAlreadyExistsAndChangeModifiedOn(ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var postReaction = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Like,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.ReplyReactions.AddAsync(postReaction);
            await db.SaveChangesAsync();

            var replyReactionsService = new ReplyReactionsService(db, dateTimeProvider.Object);
            var result = await replyReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.ReplyReactions.FirstOrDefaultAsync();
            var expected = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = dateTimeProvider.Object.Now(),
                ModifiedOn = dateTimeProvider.Object.Now()
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionsCountServiceModel>();
        }

        [Theory]
        [InlineData(ReactionType.Like)]
        [InlineData(ReactionType.Love)]
        [InlineData(ReactionType.Haha)]
        [InlineData(ReactionType.Wow)]
        [InlineData(ReactionType.Sad)]
        [InlineData(ReactionType.Angry)]
        public async Task ReactMethodShouldChangeReactionToNeutralIfReactionIsClickedTwice(ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var replyReaction = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.ReplyReactions.AddAsync(replyReaction);
            await db.SaveChangesAsync();

            var replyReactionsService = new ReplyReactionsService(db, dateTimeProvider.Object);
            var result = await replyReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.ReplyReactions.FirstOrDefaultAsync();
            var expected = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Neutral,
                CreatedOn = dateTimeProvider.Object.Now(),
                ModifiedOn = dateTimeProvider.Object.Now()
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionsCountServiceModel>();
        }

        [Fact]
        public async Task GetTotalCountMethodShouldReturnAllPostReactionsCount()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var reply = new Reply
            {
                Id = 1,
                Description = "Test description",
                AuthorId = guid,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var replyReaction = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Like,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Replies.AddAsync(reply);
            await db.ReplyReactions.AddAsync(replyReaction);
            await db.SaveChangesAsync();

            var replyReactionsService = new ReplyReactionsService(db, dateTimeProvider.Object);
            var count = await replyReactionsService.GetTotalCountAsync();

            count.Should().Be(1);
        }
    }
}