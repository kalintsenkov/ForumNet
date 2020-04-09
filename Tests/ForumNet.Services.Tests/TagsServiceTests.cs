namespace ForumNet.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Contracts;
    using Data;
    using Data.Models;

    public class TagsServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldAddOnlyOneTagInDatabase()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            await tagsService.CreateAsync("Test");

            db.Tags.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateMethodShouldAddRightTagInDatabase()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            await tagsService.CreateAsync("Test");

            var expected = new Category
            {
                Id = 1,
                Name = "Test",
                IsDeleted = false,
                CreatedOn = dateTimeProvider.Object.Now(),
            };

            var actual = await db.Tags.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test 1")]
        [InlineData("Test 2")]
        [InlineData("Test 3")]
        public async Task DeleteMethodShouldChangeIsDeletedAndDeletedOn(string name)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Tags.AddAsync(new Tag
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            await tagsService.DeleteAsync(1);

            var actual = await db.Tags.FirstOrDefaultAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().Be(dateTimeProvider.Object.Now());
        }

        [Fact]
        public async Task IsExistingMethodWithIdParameterShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Tags.AddAsync(new Tag
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            var isExisting = await tagsService.IsExistingAsync(1);

            isExisting.Should().BeTrue();
        }

        [Fact]
        public async Task IsExistingMethodWithIdParameterShouldReturnFalseIfNotExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            var isExisting = await tagsService.IsExistingAsync(1);

            isExisting.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test 1")]
        [InlineData("Test 2")]
        [InlineData("Test 3")]
        public async Task IsExistingMethodWithNameParameterShouldReturnTrueIfExists(string name)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Tags.AddAsync(new Tag
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            var isExisting = await tagsService.IsExistingAsync(name);

            isExisting.Should().BeTrue();
        }

        [Theory]
        [InlineData("Test 1")]
        [InlineData("Test 2")]
        [InlineData("Test 3")]
        public async Task IsExistingMethodWithNameParameterShouldReturnFalseIfNotExists(string name)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            var isExisting = await tagsService.IsExistingAsync(name);

            isExisting.Should().BeFalse();
        }

        [Fact]
        public async Task AreExistingMethodShouldReturnTrueIfIdsExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));


            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    CreatedOn = dateTimeProvider.Object.Now()
                });
            }

            await db.SaveChangesAsync();

            var ids = new[] { 1, 2, 3 };

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            var areExisting = await tagsService.AreExistingAsync(ids);

            areExisting.Should().BeTrue();
        }

        [Fact]
        public async Task AreExistingMethodShouldReturnFalseIfIdNotExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    CreatedOn = dateTimeProvider.Object.Now()
                });
            }

            await db.SaveChangesAsync();

            var ids = new[] { 1, 2, 3, 4, 5 };

            var tagsService = new TagsService(db, null, dateTimeProvider.Object);
            var areExisting = await tagsService.AreExistingAsync(ids);

            areExisting.Should().BeFalse();
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnCorrectModel()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Tags.AddAsync(new Tag
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetByIdAsync<Tag>(1);
            var expected = await db.Tags.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullIfTagIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetByIdAsync<Tag>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullIfTagIsDeleted()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Tags.AddAsync(new Tag
            {
                Name = "Test",
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetByIdAsync<Tag>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetAllMethodShouldAllTagsWithReturnCorrectType()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    CreatedOn = dateTimeProvider.Object.Now()
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllAsync<Tag>();
            var expected = await db.Tags.ToListAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAllMethodShouldWorkCorrectWithSearch()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tags = new List<Tag>
            {
                new Tag { Name = "Tag", CreatedOn = dateTimeProvider.Object.Now() },
                new Tag { Name = "Test 1", CreatedOn = dateTimeProvider.Object.Now() },
                new Tag { Name = "Test 2", CreatedOn = dateTimeProvider.Object.Now() }
            };

            await db.Tags.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var expected = new List<Tag>
            {
                new Tag { Id = 2, Name = "Test 1", CreatedOn = dateTimeProvider.Object.Now() },
                new Tag { Id = 3, Name = "Test 2", CreatedOn = dateTimeProvider.Object.Now() }
            };

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllAsync<Tag>("Test");

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAllMethodShouldNotReturnDeleted()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now()
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllAsync<Tag>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllMethodShouldReturnZeroItemsIfThereAreNotAnyTags()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllAsync<Tag>();

            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllByPostIdMethodShouldWorkCorrectAndReturnCorrectType(int postId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Posts = new List<PostTag> { new PostTag { PostId = postId } }
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllByPostIdAsync<Tag>(postId);
            var expected = await db.PostsTags
                .Where(pt => pt.PostId == postId)
                .Select(pt => pt.Tag)
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllByPostIdMethodShouldReturnZeroItemsIfThereAreNotAnyTagsWithThisPostId(int postId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Posts = new List<PostTag> { new PostTag { PostId = postId } }
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllByPostIdAsync<Tag>(postId);

            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllByPostIdMethodShouldNotReturnDeletedTags(int postId)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            for (int i = 0; i < 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Posts = new List<PostTag> { new PostTag { PostId = postId } }
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllByPostIdAsync<Tag>(postId);

            actual.Should().BeEmpty();
        }
    }
}
