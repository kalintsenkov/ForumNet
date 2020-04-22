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

    public class PostReactionsServiceTests
    {
        [Theory]
        [InlineData("Test 1", "Description 1", ReactionType.Like)]
        [InlineData("Test 2", "Description 2", ReactionType.Love)]
        [InlineData("Test 3", "Description 3", ReactionType.Haha)]
        [InlineData("Test 4", "Description 4", ReactionType.Wow)]
        [InlineData("Test 5", "Description 5", ReactionType.Sad)]
        [InlineData("Test 6", "Description 6", ReactionType.Angry)]
        public async Task ReactMethodShouldAddReactionInDatabaseIfNotExistsAlready(string title, string description, ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var post = new Post
            {
                Id = 1,
                Title = title,
                Description = description,
                CategoryId = 1,
                AuthorId = guid,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionsService(db, dateTimeProvider.Object);
            var result = await postReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.PostReactions.FirstOrDefaultAsync();
            var expected = new PostReaction
            {
                Id = 1,
                PostId = 1,
                Post = post,
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
            
            var postReaction = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Like,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.PostReactions.AddAsync(postReaction);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionsService(db, dateTimeProvider.Object);
            var result = await postReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.PostReactions.FirstOrDefaultAsync();
            var expected = new PostReaction
            {
                Id = 1,
                PostId = 1,
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

            var postReaction = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.PostReactions.AddAsync(postReaction);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionsService(db, dateTimeProvider.Object);
            var result = await postReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.PostReactions.FirstOrDefaultAsync();
            var expected = new PostReaction
            {
                Id = 1,
                PostId = 1,
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

            var post = new Post
            {
                Id = 1,
                Title = "Test title",
                Description = "Test description",
                CategoryId = 1,
                AuthorId = guid,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var postReaction = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Like,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Posts.AddAsync(post);
            await db.PostReactions.AddAsync(postReaction);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionsService(db, dateTimeProvider.Object);
            var count = await postReactionsService.GetTotalCountAsync();

            count.Should().Be(1);
        }
    }
}