namespace ForumNet.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Common;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Users;

    public class UsersServiceTests
    {
        [Theory]
        [InlineData("Test1", "test1@test.com", GenderType.Male)]
        [InlineData("Test2", "test2@test.com", GenderType.Female)]
        [InlineData("Test3", "test3@test.com", GenderType.NotKnown)]
        public async Task ModifyMethodShouldChangeModifiedOn(string userName, string email, GenderType gender)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expected = new ForumUser
            {
                Id = guid,
                UserName = userName,
                Email = email,
                ProfilePicture = "Test",
                Gender = gender,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.ModifyAsync(guid);

            var actual = await db.Users.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Test1", "test1@test.com", GenderType.Male)]
        [InlineData("Test2", "test2@test.com", GenderType.Female)]
        [InlineData("Test3", "test3@test.com", GenderType.NotKnown)]
        public async Task DeleteMethodShouldChangeIsDeletedAndDeletedOn(string userName, string email, GenderType gender)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = guid,
                UserName = userName,
                Email = email,
                ProfilePicture = "Test",
                Gender = gender,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.DeleteAsync(guid);

            var actual = await db.Users.FirstOrDefaultAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().BeSameDateAs(dateTimeProvider.Object.Now());
        }

        [Theory]
        [InlineData("Test1", "test1@test.com", GenderType.Male)]
        [InlineData("Test2", "test2@test.com", GenderType.Female)]
        [InlineData("Test3", "test3@test.com", GenderType.NotKnown)]
        public async Task DeleteMethodShouldSetEmailToNull(string userName, string email, GenderType gender)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = guid,
                UserName = userName,
                Email = email,
                ProfilePicture = "Test",
                Gender = gender,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.DeleteAsync(guid);

            var actual = await db.Users.FirstOrDefaultAsync();

            actual.Email.Should().BeNull();
            actual.NormalizedEmail.Should().BeNull();
        }

        [Theory]
        [InlineData(5, 1)]
        [InlineData(1, 5)]
        public async Task AddPointsMethodShouldIncreaseUserPoints(int points, int pointsToAdd)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = guid,
                UserName = "Test",
                Email = "test@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                Points = points,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.AddPointsAsync(guid, pointsToAdd);

            var actual = await db.Users.FirstOrDefaultAsync();

            actual.Points.Should().Be(6);
        }

        [Theory]
        [InlineData("User1", "Follower1")]
        [InlineData("User2", "Follower2")]
        [InlineData("User3", "Follower3")]
        public async Task FollowMethodShouldAddFollowInformationInDatabaseAndReturnTrueIfUserFollowSomeoneSuccessfully(string userId, string followerId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var isFollowed = await usersService.FollowAsync(userId, followerId);

            var actual = await db.UsersFollowers.FirstOrDefaultAsync();

            actual.UserId.Should().Be(userId);
            actual.FollowerId.Should().Be(followerId);
            isFollowed.Should().BeTrue();
        }

        [Theory]
        [InlineData("User1", "Follower1")]
        [InlineData("User2", "Follower2")]
        [InlineData("User3", "Follower3")]
        public async Task FollowMethodShouldReturnTrueIfUserFollowSomeoneAgainAndModifiedOnShouldBeSetToCurrentDate(string userId, string followerId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.FollowAsync(userId, followerId);
            await usersService.FollowAsync(userId, followerId);
            var isFollowed = await usersService.FollowAsync(userId, followerId);

            var actual = await db.UsersFollowers.FirstOrDefaultAsync();

            actual.UserId.Should().Be(userId);
            actual.FollowerId.Should().Be(followerId);
            actual.ModifiedOn.Should().BeSameDateAs(dateTimeProvider.Object.Now());
            isFollowed.Should().BeTrue();
        }

        [Theory]
        [InlineData("User1", "Follower1")]
        [InlineData("User2", "Follower2")]
        [InlineData("User3", "Follower3")]
        public async Task FollowMethodShouldReturnFalseIfUserUnfollowSomeoneAndFollowInformationInDatabaseToDeleted(string userId, string followerId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.FollowAsync(userId, followerId);
            var isFollowed = await usersService.FollowAsync(userId, followerId);

            var actual = await db.UsersFollowers.FirstOrDefaultAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().BeSameDateAs(dateTimeProvider.Object.Now());
            isFollowed.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test1")]
        [InlineData("Test2")]
        [InlineData("Test3")]
        public async Task IsUsernameUsedMethodShouldReturnTrueIfUsernameIsUsed(string username)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = guid,
                UserName = username,
                Email = "test@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var isUsernameUsed = await usersService.IsUsernameUsedAsync(username);

            isUsernameUsed.Should().BeTrue();
        }

        [Theory]
        [InlineData("Test1")]
        [InlineData("Test2")]
        [InlineData("Test3")]
        public async Task IsUsernameUsedMethodShouldReturnFalseIfUsernameIsNotUsed(string username)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var isUsernameUsed = await usersService.IsUsernameUsedAsync(username);

            isUsernameUsed.Should().BeFalse();
        }

        [Theory]
        [InlineData("Test1")]
        [InlineData("Test2")]
        [InlineData("Test3")]
        public async Task IsDeletedMethodShouldReturnTrueIfUserIsDeleted(string username)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = guid,
                UserName = username,
                Email = "test@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now(),
                DeletedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var isDeleted = await usersService.IsDeletedAsync(username);

            isDeleted.Should().BeTrue();
        }

        [Theory]
        [InlineData("Test1")]
        [InlineData("Test2")]
        [InlineData("Test3")]
        public async Task IsDeletedMethodShouldReturnFalseIfUserIsNotDeleted(string username)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = guid,
                UserName = username,
                Email = "test@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now(),
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var isDeleted = await usersService.IsDeletedAsync(username);

            isDeleted.Should().BeFalse();
        }

        [Theory]
        [InlineData("User1", "Follower1")]
        [InlineData("User2", "Follower2")]
        [InlineData("User3", "Follower3")]
        public async Task IsFollowedAlreadyShouldReturnTrueIfUserAlreadyFollowCurrentUser(string userId, string followerId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            await usersService.FollowAsync(userId, followerId);
            var isFollowed = await usersService.IsFollowedAlreadyAsync(userId, followerId);

            isFollowed.Should().BeTrue();
        }

        [Theory]
        [InlineData("User1", "Follower1")]
        [InlineData("User2", "Follower2")]
        [InlineData("User3", "Follower3")]
        public async Task IsFollowedAlreadyShouldReturnFalseIfUserDoNotFollowCurrentUser(string userId, string followerId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var isFollowed = await usersService.IsFollowedAlreadyAsync(userId, followerId);

            isFollowed.Should().BeFalse();
        }

        [Fact]
        public async Task GetTotalCountMethodShouldReturnTotalRegisteredUsers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var users = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "User1",
                    UserName = "User1",
                    Email = "user1@test.com",
                    ProfilePicture = "Test",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                },
                new ForumUser
                {
                    Id = "User2",
                    UserName = "User2",
                    Email = "user2@test.com",
                    ProfilePicture = "Test",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                }
            };

            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var usersCount = await usersService.GetTotalCountAsync();

            usersCount.Should().Be(users.Count);
        }

        [Fact]
        public async Task GetTotalCountMethodShouldNotCountDeletedUsers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var users = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "User1",
                    UserName = "User1",
                    Email = "user1@test.com",
                    ProfilePicture = "Test",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "User2",
                    UserName = "User2",
                    Email = "user2@test.com",
                    ProfilePicture = "Test",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                }
            };

            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var usersCount = await usersService.GetTotalCountAsync();

            usersCount.Should().Be(0);
        }

        [Fact]
        public async Task GetFollowersCountShouldCountUserFollowers()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "123",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var follower = new ForumUser
            {
                Id = "321",
                UserName = "follower",
                Email = "follower@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.Users.AddAsync(follower);

            var userFollower = new UserFollower
            {
                UserId = user.Id,
                FollowerId = follower.Id,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.UsersFollowers.AddAsync(userFollower);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var followersCount = await usersService.GetFollowersCountAsync(user.Id);

            followersCount.Should().Be(1);
        }

        [Fact]
        public async Task GetFollowersCountShouldCountUserFollowersButNotDeletedUsers()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "123",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var follower = new ForumUser
            {
                Id = "321",
                UserName = "follower",
                Email = "follower@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now(),
                DeletedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.Users.AddAsync(follower);

            var userFollower = new UserFollower
            {
                UserId = user.Id,
                FollowerId = follower.Id,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.UsersFollowers.AddAsync(userFollower);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var followersCount = await usersService.GetFollowersCountAsync(user.Id);

            followersCount.Should().Be(0);
        }

        [Fact]
        public async Task GetFollowingCountShouldCountUserFollowings()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "123",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var following = new ForumUser
            {
                Id = "321",
                UserName = "following",
                Email = "following@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.Users.AddAsync(following);

            var userFollower = new UserFollower
            {
                UserId = following.Id,
                FollowerId = user.Id,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.UsersFollowers.AddAsync(userFollower);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var followingCount = await usersService.GetFollowingCountAsync(user.Id);

            followingCount.Should().Be(1);
        }

        [Fact]
        public async Task GetFollowingCountShouldCountUserFollowingsButNotDeletedUsers()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "123",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var following = new ForumUser
            {
                Id = "321",
                UserName = "following",
                Email = "following@test.com",
                ProfilePicture = "Test",
                Gender = GenderType.NotKnown,
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now(),
                DeletedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(user);
            await db.Users.AddAsync(following);

            var userFollower = new UserFollower
            {
                UserId = following.Id,
                FollowerId = user.Id,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.UsersFollowers.AddAsync(userFollower);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, null, dateTimeProvider.Object);
            var followingCount = await usersService.GetFollowingCountAsync(user.Id);

            followingCount.Should().Be(0);
        }

        [Theory]
        [InlineData("user1", "user1@test.com", GenderType.NotKnown)]
        [InlineData("user2", "user2@test.com", GenderType.Male)]
        [InlineData("user3", "user3@test.com", GenderType.Female)]
        public async Task GetByIdShouldReturnCorrectModel(string userName, string email, GenderType gender)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expected = new ForumUser
            {
                Id = guid,
                UserName = userName,
                Email = email,
                ProfilePicture = "Some picture",
                Gender = gender,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetByIdAsync<ForumUser>(guid);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("user1", "user1@test.com", GenderType.NotKnown)]
        [InlineData("user2", "user2@test.com", GenderType.Male)]
        [InlineData("user3", "user3@test.com", GenderType.Female)]
        public async Task GetByIdShouldReturnNullIfUserIsDeleted(string userName, string email, GenderType gender)
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expected = new ForumUser
            {
                Id = guid,
                UserName = userName,
                Email = email,
                ProfilePicture = "Some picture",
                Gender = gender,
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now(),
                DeletedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetByIdAsync<ForumUser>(guid);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetAllMethodShouldReturnCorrectModels()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now()
                }
            };

            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetAllAsync<ForumUser>();

            actual.Should().BeEquivalentTo(expected);
            actual.Should().HaveCount(expected.Count);
        }

        [Fact]
        public async Task GetAllMethodShouldNotReturnDeletedUsers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var users = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now()
                }
            };

            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetAllAsync<ForumUser>();

            actual.Should().HaveCount(1);
            actual.FirstOrDefault().Should().BeEquivalentTo(users[1]);
        }

        [Fact]
        public async Task GetAdminsMethodShouldReturnOnlyAdmins()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var role = new ForumRole
            {
                Id = "Some role id",
                Name = GlobalConstants.AdministratorRoleName,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Roles.AddAsync(role);

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "Some role id"
                        }
                    }
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "Some role id"
                        }
                    }
                }
            };

            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetAdminsAsync<ForumUser>();

            actual.Should().BeEquivalentTo(expected);
            actual.Should().HaveCount(expected.Count);
        }

        [Fact]
        public async Task GetAdminsMethodShouldNotReturnDeleted()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var role = new ForumRole
            {
                Id = "Some role id",
                Name = GlobalConstants.AdministratorRoleName,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            await db.Roles.AddAsync(role);

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "Some role id"
                        }
                    }
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now(),
                    Roles = new List<IdentityUserRole<string>>
                    {
                        new IdentityUserRole<string>
                        {
                            RoleId = "Some role id"
                        }
                    }
                }
            };

            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetAdminsAsync<ForumUser>();

            actual.Should().HaveCount(1);
            actual.FirstOrDefault().Should().BeEquivalentTo(expected[0]);
        }

        [Fact]
        public async Task GetFollowersMethodShouldReturnUserFollowers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "12",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Some picture",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                }
            };

            await db.Users.AddAsync(user);
            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            await usersService.FollowAsync("12", "123");
            await usersService.FollowAsync("12", "1234");

            var actual = await usersService.GetFollowersAsync<ForumUser>("12");

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetFollowersMethodShouldNotReturnDeletedUsers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "12",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Some picture",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                }
            };

            await db.Users.AddAsync(user);
            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            await usersService.FollowAsync("12", "123");
            await usersService.FollowAsync("12", "1234");

            var actual = await usersService.GetFollowersAsync<ForumUser>("12");

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetFollowingMethodShouldReturnUserFollowing()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "12",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Some picture",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    CreatedOn = dateTimeProvider.Object.Now(),
                }
            };

            await db.Users.AddAsync(user);
            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            await usersService.FollowAsync("123", "12");
            await usersService.FollowAsync("1234", "12");

            var actual = await usersService.GetFollowingAsync<ForumUser>("12");
            
            var actualUser1 = actual.ToList()[0];
            var actualUser2 = actual.ToList()[1];
            var expectedUser1 = expected[0];
            var expectedUser2 = expected[1];

            actualUser1.Id.Should().Be(expectedUser1.Id);
            actualUser1.UserName.Should().Be(expectedUser1.UserName);
            actualUser2.Id.Should().Be(expectedUser2.Id);
            actualUser2.UserName.Should().Be(expectedUser2.UserName);
        }

        [Fact]
        public async Task GetFollowingMethodShouldNotReturnDeletedUsers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForumUser, ForumUser>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var user = new ForumUser
            {
                Id = "12",
                UserName = "user",
                Email = "user@test.com",
                ProfilePicture = "Some picture",
                Gender = GenderType.NotKnown,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var expected = new List<ForumUser>
            {
                new ForumUser
                {
                    Id = "123",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                },
                new ForumUser
                {
                    Id = "1234",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    ProfilePicture = "Some picture",
                    Gender = GenderType.NotKnown,
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                }
            };

            await db.Users.AddAsync(user);
            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new UsersService(db, mapper, dateTimeProvider.Object);
            await usersService.FollowAsync("123", "12");
            await usersService.FollowAsync("1234", "12");

            var actual = await usersService.GetFollowingAsync<ForumUser>("12");

            actual.Should().BeEmpty();
        }
    }
}
