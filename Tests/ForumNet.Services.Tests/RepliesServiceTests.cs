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

    public class RepliesServiceTests
    {
        [Theory]
        [InlineData("Test 1", 1, 1)]
        [InlineData("Test 2", null, 1)]
        public async Task CreateMethodShouldAddOnlyOneReplyInDatabase(string description, int? parentId, int postId)
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
        public async Task IsExistingMethodShouldReturnTrueIfExists()
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
                CreatedOn = dateTimeProviderMock.Object.Now()
            });
            await db.SaveChangesAsync();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            var isExisting = await repliesService.IsExistingAsync(1);

            isExisting.Should().BeTrue();
        }

        [Fact]
        public async Task IsExistingMethodShouldReturnFalseIfNotExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var usersServiceMock = new Mock<IUsersService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var repliesService = new RepliesService(db, null, usersServiceMock.Object, dateTimeProviderMock.Object);
            var isExisting = await repliesService.IsExistingAsync(1);

            isExisting.Should().BeFalse();
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

        //[Fact]
        //public async Task GetByIdMethodShouldReturnCorrectModel()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Reply, Reply>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    await db.Replies.AddAsync(new Reply
        //    {
        //        Description = "Test",
        //        CreatedOn = dateTimeProvider.Object.Now()
        //    });
        //    await db.SaveChangesAsync();

        //    var repliesService = new RepliesService(db, mapper, null, dateTimeProvider.Object);
        //    var expected = await repliesService.GetByIdAsync<Reply>(1);
        //    var actual = await db.Replies.FirstOrDefaultAsync();

        //    actual.Description.Should().Be(expected.Description);
        //    actual.CreatedOn.Should().BeSameDateAs(expected.CreatedOn);
        //}

        //[Fact]
        //public async Task GetAllByUserIdMethodShouldWorksCorrectly()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);
        //    var usersServiceMock = new Mock<IUsersService>();
        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Reply, Reply>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var expectedReplies = new List<Reply>();

        //    for (int i = 0; i < 10; i++)
        //    {
        //        expectedReplies.Add(new Reply
        //        {
        //            Description = $"Test {i}",
        //            ParentId = null,
        //            PostId = 1,
        //            AuthorId = "123",
        //            CreatedOn = dateTimeProvider.Object.Now()
        //        });
        //    }

        //    await db.Replies.AddRangeAsync(expectedReplies);
        //    await db.SaveChangesAsync();

        //    var repliesService = new RepliesService(db, mapper, usersServiceMock.Object, dateTimeProvider.Object);
        //    var actualReplies = await repliesService.GetAllByUserIdAsync<Reply>("123");

        //    expectedReplies.Should().BeEquivalentTo(actualReplies);
        //}
    }
}
