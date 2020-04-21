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
    using Providers;
    using Reports;

    public class PostReportsServiceTests
    {
        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task CreateMethodShouldAddPostReportInDatabase(string description, int postId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var postReportsService = new PostReportsService(db, null, dateTimeProvider.Object);
            await postReportsService.CreateAsync(description, postId, Guid.NewGuid().ToString());

            db.PostReports.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task CreateMethodShouldAddRightPostReportInDatabase(string description, int postId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var postReportsService = new PostReportsService(db, null, dateTimeProvider.Object);

            var authorId = Guid.NewGuid().ToString();
            await postReportsService.CreateAsync(description, postId, authorId);

            var expected = new PostReport
            {
                Id = 1,
                Description = description,
                PostId = postId,
                AuthorId = authorId,
                CreatedOn = dateTimeProvider.Object.Now(),
            };

            var actual = await db.PostReports.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task DeleteMethodShouldChangeIsDeletedAndDeletedOn(string description, int postId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.PostReports.AddAsync(new PostReport
            {
                Description = description,
                PostId = postId,
                AuthorId = Guid.NewGuid().ToString(),
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var postReportsService = new PostReportsService(db, null, dateTimeProvider.Object);
            await postReportsService.DeleteAsync(1);

            var actual = await db.PostReports.FirstOrDefaultAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTimeProvider.Object.Now());
        }

        [Fact]
        public async Task IsExistingMethodShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.PostReports.AddAsync(new PostReport
            {
                Description = "Test",
                PostId = 1,
                AuthorId = Guid.NewGuid().ToString(),
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var postReportsService = new PostReportsService(db, null, dateTimeProvider.Object);
            var isExisting = await postReportsService.IsExistingAsync(1);

            isExisting.Should().BeTrue();
        }

        [Fact]
        public async Task IsExistingMethodShouldReturnFalseIfNotExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var postReportsService = new PostReportsService(db, null, dateTimeProvider.Object);
            var isExisting = await postReportsService.IsExistingAsync(1);

            isExisting.Should().BeFalse();
        }
    }
}