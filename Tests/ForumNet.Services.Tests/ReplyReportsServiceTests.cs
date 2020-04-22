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
    using Providers.DateTime;
    using Reports;

    public class ReplyReportsServiceTests
    {
        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task CreateMethodShouldAddReplyReportInDatabase(string description, int replyId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var replyReportsService = new ReplyReportsService(db, null, dateTimeProvider.Object);
            await replyReportsService.CreateAsync(description, replyId, Guid.NewGuid().ToString());

            db.ReplyReports.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task CreateMethodShouldAddRightReplyReportInDatabase(string description, int replyId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var postReportsService = new ReplyReportsService(db, null, dateTimeProvider.Object);

            var authorId = Guid.NewGuid().ToString();
            await postReportsService.CreateAsync(description, replyId, authorId);

            var expected = new ReplyReport
            {
                Id = 1,
                Description = description,
                ReplyId = replyId,
                AuthorId = authorId,
                CreatedOn = dateTimeProvider.Object.Now(),
            };

            var actual = await db.ReplyReports.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task DeleteMethodShouldChangeIsDeletedAndDeletedOn(string description, int replyId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.ReplyReports.AddAsync(new ReplyReport
            {
                Description = description,
                ReplyId = replyId,
                AuthorId = Guid.NewGuid().ToString(),
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var replyReportsService = new ReplyReportsService(db, null, dateTimeProvider.Object);
            await replyReportsService.DeleteAsync(1);

            var actual = await db.ReplyReports.FirstOrDefaultAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTimeProvider.Object.Now());
        }

        [Fact]
        public async Task DeleteMethodShouldReturnFalseIfReportIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var replyReportsService = new ReplyReportsService(db, null, dateTimeProvider.Object);
            var deleted = await replyReportsService.DeleteAsync(1);

            deleted.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task DeleteMethodShouldReturnTrueIfReportIsDeleted(string description, int replyId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.ReplyReports.AddAsync(new ReplyReport
            {
                Description = description,
                ReplyId = replyId,
                AuthorId = Guid.NewGuid().ToString(),
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var replyReportsService = new ReplyReportsService(db, null, dateTimeProvider.Object);
            var deleted = await replyReportsService.DeleteAsync(1);

            deleted.Should().BeTrue();
        }
    }
}