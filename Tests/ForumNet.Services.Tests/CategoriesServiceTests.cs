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

    using Categories;
    using Data;
    using Data.Models;
    using Providers;

    public class CategoriesServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldAddCategoryInDatabase()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.CreateAsync("Test");

            db.Categories.Should().HaveCount(1);
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

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.CreateAsync("Test");

            var expected = new Category
            {
                Id = 1,
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now(),
                IsDeleted = false
            };

            var actual = await db.Categories.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test 1", "Edit 1")]
        [InlineData("Test 2", "Edit 2")]
        [InlineData("Test 3", "Edit 3")]
        public async Task EditMethodShouldChangeCategoryNameAndModifiedOn(string creationName, string editedName)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = creationName,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.EditAsync(1, editedName);

            var expected = new Category
            {
                Name = editedName,
                ModifiedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Categories.FirstOrDefaultAsync();

            actual.Name.Should().BeSameAs(expected.Name);
            actual.ModifiedOn.Should().Be(expected.ModifiedOn);
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

            await db.Categories.AddAsync(new Category
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.DeleteAsync(1);

            var actual = await db.Categories.FirstOrDefaultAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().BeSameDateAs(dateTimeProvider.Object.Now());
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

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(1);

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

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(1);

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

            await db.Categories.AddAsync(new Category
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(name);

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

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(name);

            isExisting.Should().BeFalse();
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
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetByIdAsync<Category>(1);
            var actual = await db.Categories.FirstOrDefaultAsync();

            actual.Id.Should().Be(expected.Id);
            actual.Name.Should().BeSameAs(expected.Name);
            actual.CreatedOn.Should().BeSameDateAs(expected.CreatedOn);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnModelWhichIsNotDeleted()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetByIdAsync<Category>(1);
            var actual = await db.Categories.FirstOrDefaultAsync();

            actual.IsDeleted.Should().Be(expected.IsDeleted);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullWhenCategoryIsDeleted()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now(),
                IsDeleted = true,
                DeletedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetByIdAsync<Category>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullWhenCategoryIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetByIdAsync<Category>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetAllMethodShouldReturnCorrectModel()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expectedCategories = new List<Category>();

            for (int i = 0; i < 10; i++)
            {
                expectedCategories.Add(new Category
                {
                    Name = $"Test {i}",
                    CreatedOn = dateTimeProvider.Object.Now()
                });
            }

            await db.Categories.AddRangeAsync(expectedCategories);
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllAsync<Category>();

            actualCategories.Should().BeEquivalentTo(expectedCategories).And.AllBeOfType<Category>();
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
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expectedCategories = new List<Category>();

            for (int i = 0; i < 5; i++)
            {
                expectedCategories.Add(new Category
                {
                    Name = $"Test {i}",
                    IsDeleted = true,
                    DeletedOn = dateTimeProvider.Object.Now(),
                    CreatedOn = dateTimeProvider.Object.Now(),
                });
            }

            await db.Categories.AddRangeAsync(expectedCategories);
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetAllAsync<Category>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllMethodShouldReturnZeroItemsIfThereAreNotAnyCategories()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetAllAsync<Category>();

            actual.Should().BeEmpty();
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
                cfg.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categories = new List<Category>
            {
                new Category { Name = "Category", CreatedOn = dateTimeProvider.Object.Now() },
                new Category { Name = "Test 1", CreatedOn = dateTimeProvider.Object.Now() },
                new Category { Name = "Test 2", CreatedOn = dateTimeProvider.Object.Now() }
            };

            await db.Categories.AddRangeAsync(categories);
            await db.SaveChangesAsync();

            var expectedCategories = new List<Category>
            {
                new Category { Id = 2, Name = "Test 1", CreatedOn = dateTimeProvider.Object.Now() },
                new Category { Id = 3, Name = "Test 2", CreatedOn = dateTimeProvider.Object.Now() }
            };

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllAsync<Category>("Test");

            actualCategories.Should().BeEquivalentTo(expectedCategories).And.AllBeOfType<Category>();
        }
    }
}
