namespace ForumNet.Services.Tests
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Contracts;
    using Data;
    using Data.Models;

    public class PostReportsServiceTests
    {
        [Theory]
        [InlineData("Test 1", 1)]
        [InlineData("Test 2", 2)]
        [InlineData("Test 3", 3)]
        public async Task CreateMethodShouldAddOnlyOnePostReportInDatabase(string description, int postId)
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

        //[Fact]
        //public async Task GetByIdMethodShouldReturnCorrectModel()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<PostReport, PostReport>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    await db.PostReports.AddAsync(new PostReport
        //    {
        //        Description = "Test",
        //        PostId = 1,
        //        AuthorId = "123",
        //        IsDeleted = false,
        //        CreatedOn = dateTimeProvider.Object.Now()
        //    });
        //    await db.SaveChangesAsync();

        //    var postReportsService = new PostReportsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await postReportsService.GetByIdAsync<PostReport>(1);
        //    var expected = await db.PostReports.FirstOrDefaultAsync();

        //    actual.Should().BeEquivalentTo(expected);
        //}

        //[Fact]
        //public async Task GetByIdMethodShouldReturnNullIfTagIsNotFound()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetByIdAsync<Tag>(1);

        //    actual.Should().BeNull();
        //}

        //[Fact]
        //public async Task GetByIdMethodShouldReturnNullIfTagIsDeleted()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    await db.Tags.AddAsync(new Tag
        //    {
        //        Name = "Test",
        //        IsDeleted = true,
        //        CreatedOn = dateTimeProvider.Object.Now()
        //    });
        //    await db.SaveChangesAsync();

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetByIdAsync<Tag>(1);

        //    actual.Should().BeNull();
        //}

        //[Fact]
        //public async Task GetAllMethodShouldAllTagsWithReturnCorrectType()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    for (int i = 0; i < 3; i++)
        //    {
        //        await db.Tags.AddAsync(new Tag
        //        {
        //            Name = $"Test {i}",
        //            CreatedOn = dateTimeProvider.Object.Now()
        //        });
        //    }
        //    await db.SaveChangesAsync();

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllAsync<Tag>();
        //    var expected = await db.Tags.ToListAsync();

        //    actual.Should().BeEquivalentTo(expected);
        //}

        //[Fact]
        //public async Task GetAllMethodShouldWorkCorrectWithSearch()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    var tags = new List<Tag>
        //    {
        //        new Tag { Name = "Tag", CreatedOn = dateTimeProvider.Object.Now() },
        //        new Tag { Name = "Test 1", CreatedOn = dateTimeProvider.Object.Now() },
        //        new Tag { Name = "Test 2", CreatedOn = dateTimeProvider.Object.Now() }
        //    };

        //    await db.Tags.AddRangeAsync(tags);
        //    await db.SaveChangesAsync();

        //    var expected = new List<Tag>
        //    {
        //        new Tag { Id = 2, Name = "Test 1", CreatedOn = dateTimeProvider.Object.Now() },
        //        new Tag { Id = 3, Name = "Test 2", CreatedOn = dateTimeProvider.Object.Now() }
        //    };

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllAsync<Tag>("Test");

        //    actual.Should().BeEquivalentTo(expected);
        //}

        //[Fact]
        //public async Task GetAllMethodShouldNotReturnDeleted()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    for (int i = 0; i < 3; i++)
        //    {
        //        await db.Tags.AddAsync(new Tag
        //        {
        //            Name = $"Test {i}",
        //            IsDeleted = true,
        //            CreatedOn = dateTimeProvider.Object.Now()
        //        });
        //    }
        //    await db.SaveChangesAsync();

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllAsync<Tag>();

        //    actual.Should().BeEmpty();
        //}

        //[Fact]
        //public async Task GetAllMethodShouldReturnZeroItemsIfThereAreNotAnyTags()
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllAsync<Tag>();

        //    actual.Should().BeEmpty();
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //public async Task GetAllByPostIdMethodShouldWorkCorrectAndReturnCorrectType(int postId)
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    for (int i = 0; i < 3; i++)
        //    {
        //        await db.Tags.AddAsync(new Tag
        //        {
        //            Name = $"Test {i}",
        //            IsDeleted = true,
        //            CreatedOn = dateTimeProvider.Object.Now(),
        //            Posts = new List<PostTag> { new PostTag { PostId = postId } }
        //        });
        //    }
        //    await db.SaveChangesAsync();

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllByPostIdAsync<Tag>(postId);
        //    var expected = await db.PostsTags
        //        .Where(pt => pt.PostId == postId)
        //        .Select(pt => pt.Tag)
        //        .Where(t => !t.IsDeleted)
        //        .ToListAsync();

        //    actual.Should().BeEquivalentTo(expected);
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //public async Task GetAllByPostIdMethodShouldReturnZeroItemsIfThereAreNotAnyTagsWithThisPostId(int postId)
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    for (int i = 0; i < 3; i++)
        //    {
        //        await db.Tags.AddAsync(new Tag
        //        {
        //            Name = $"Test {i}",
        //            IsDeleted = true,
        //            CreatedOn = dateTimeProvider.Object.Now(),
        //            Posts = new List<PostTag> { new PostTag { PostId = postId } }
        //        });
        //    }
        //    await db.SaveChangesAsync();

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllByPostIdAsync<Tag>(postId);

        //    actual.Should().BeEmpty();
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //public async Task GetAllByPostIdMethodShouldNotReturnDeletedTags(int postId)
        //{
        //    var options = new DbContextOptionsBuilder<ForumDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .Options;

        //    var db = new ForumDbContext(options);

        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Tag, Tag>();
        //    });

        //    var mapper = config.CreateMapper();

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

        //    for (int i = 0; i < 3; i++)
        //    {
        //        await db.Tags.AddAsync(new Tag
        //        {
        //            Name = $"Test {i}",
        //            IsDeleted = true,
        //            CreatedOn = dateTimeProvider.Object.Now(),
        //            Posts = new List<PostTag> { new PostTag { PostId = postId } }
        //        });
        //    }
        //    await db.SaveChangesAsync();

        //    var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
        //    var actual = await tagsService.GetAllByPostIdAsync<Tag>(postId);

        //    actual.Should().BeEmpty();
        //}
    }
}