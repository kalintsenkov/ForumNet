﻿namespace ForumNet.Services.Tests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Data;
    using Data.Models;
    using Providers.DateTime;
    using Replies;
    using Users;

    public class RepliesServiceTests
    {
        [Theory]
        [InlineData("Test 1", 1, 1)]
        [InlineData("Test 2", null, 1)]
        public async Task CreateMethodShouldAddReplyInDatabase(string description, int? parentId, int postId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            await repliesService.CreateAsync(description, parentId, postId, guid);

            db.Replies.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Test 1", 1, 1)]
        [InlineData("Test 2", null, 1)]
        public async Task CreateMethodShouldAddRightReplyInDatabase(string description, int? parentId, int postId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            await repliesService.CreateAsync(description, parentId, postId, guid);

            var expected = new Reply
            {
                Id = 1,
                Description = description,
                ParentId = parentId,
                PostId = postId,
                AuthorId = guid,
                CreatedOn = dateTimeProviderMock.Object.Now(),
            };

            var actual = await db.Replies.FirstOrDefaultAsync();

            expected.Id.Should().Be(actual.Id);
            expected.Description.Should().Be(actual.Description);
            expected.ParentId.Should().Be(actual.ParentId);
            expected.PostId.Should().Be(actual.PostId);
            expected.AuthorId.Should().Be(actual.AuthorId);
            expected.CreatedOn.Should().BeSameDateAs(actual.CreatedOn);
        }

        [Theory]
        [InlineData("Test 1", "Edit 1")]
        [InlineData("Test 2", "Edit 2")]
        [InlineData("Test 3", "Edit 3")]
        public async Task EditMethodShouldChangeDescriptionAndModifiedOn(string creationDescription, string editedDescription)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Replies.AddAsync(new Reply
            {
                Description = creationDescription,
                CreatedOn = dateTimeProviderMock.Object.Now()
            });
            await db.SaveChangesAsync();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            await repliesService.EditAsync(1, editedDescription);

            var expected = new Reply
            {
                Description = editedDescription,
                ModifiedOn = dateTimeProviderMock.Object.Now()
            };

            var actual = await db.Replies.FirstOrDefaultAsync();

            expected.Description.Should().BeSameAs(actual.Description);
            expected.ModifiedOn.Should().Be(actual.ModifiedOn);
        }

        [Theory]
        [InlineData("Test 1")]
        [InlineData("Test 2")]
        [InlineData("Test 3")]
        public async Task DeleteMethodShouldChangeIsDeletedAndDeletedOn(string description)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Replies.AddAsync(new Reply
            {
                Description = description,
                CreatedOn = dateTimeProviderMock.Object.Now()
            });
            await db.SaveChangesAsync();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            await repliesService.DeleteAsync(1);

            var expected = new Reply
            {
                Id = 1,
                IsDeleted = true,
                DeletedOn = dateTimeProviderMock.Object.Now()
            };

            var actual = await db.Replies.FirstOrDefaultAsync();

            expected.Id.Should().Be(actual.Id);
            expected.IsDeleted.Should().Be(actual.IsDeleted);
            expected.DeletedOn.Should().Be(actual.DeletedOn);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task MakeBestAnswerMethodShouldChangeIsBestAnswer(bool isBestAnswer)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Replies.AddAsync(new Reply
            {
                Description = "Test",
                IsBestAnswer = isBestAnswer,
                CreatedOn = dateTimeProviderMock.Object.Now()
            });
            await db.SaveChangesAsync();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            await repliesService.MakeBestAnswerAsync(1);

            var expected = new Reply
            {
                Id = 1,
                Description = "Test",
                IsBestAnswer = !isBestAnswer,
                CreatedOn = dateTimeProviderMock.Object.Now()
            };

            var actual = await db.Replies.FirstOrDefaultAsync();

            expected.Id.Should().Be(actual.Id);
            expected.Description.Should().Be(actual.Description);
            expected.IsBestAnswer.Should().Be(actual.IsBestAnswer);
            expected.CreatedOn.Should().BeSameDateAs(actual.CreatedOn);
        }

        [Fact]
        public async Task GetAuthorIdByIdMethodShouldReturnCorrectId()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Replies.AddAsync(new Reply
            {
                Description = "Test",
                AuthorId = guid,
                CreatedOn = dateTimeProviderMock.Object.Now()
            });
            await db.SaveChangesAsync();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            var authorId = await repliesService.GetAuthorIdByIdAsync(1);

            authorId.Should().BeSameAs(guid);
        }

        [Fact]
        public async Task GetAuthorIdByIdMethodShouldReturnNullIfReplyIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            var authorId = await repliesService.GetAuthorIdByIdAsync(1);

            authorId.Should().BeNull();
        }
    }
}
