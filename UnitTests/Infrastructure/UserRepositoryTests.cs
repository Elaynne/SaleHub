using Domain.Models;
using FluentAssertions;
using Infrastructure.Exceptions;
using Infrastructure.Repository.User;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace UnitTests.Infrastructure
{
    public class UserRepositoryTests
    {
        private readonly IMemoryCache _memoryCache;
        private readonly UserRepository _userRepository;
        private readonly User _user = new User { Id = Guid.NewGuid(), UserName = "Test User" };
        private readonly Dictionary<Guid, User> _users = new Dictionary<Guid, User>
            {
                { Guid.NewGuid(), new User { Id = Guid.NewGuid(), UserName = "User 1" } },
                { Guid.NewGuid(), new User { Id = Guid.NewGuid(), UserName = "User 2" } }
            };
        public UserRepositoryTests()
        {
            _memoryCache = Substitute.For<IMemoryCache>();
            _userRepository = new UserRepository(_memoryCache);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserToCache()
        {
            var cacheEntry = Substitute.For<ICacheEntry>();

            _memoryCache.CreateEntry($"User_{_user.Id}").Returns(cacheEntry);

            var result = await _userRepository.AddUserAsync(_user);

            result.Should().Be(_user);
            _memoryCache.Received().Set($"User_{_user.Id}", _user, TimeSpan.FromMinutes(15));
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            _memoryCache.TryGetValue("UsersKey", out Arg.Any<Dictionary<Guid, User>>())
                .Returns(callInfo =>
                {
                    callInfo[1] = _users;
                    return true;
                });

            var result = await _userRepository.GetAllUsersAsync();
            
            result.Should().BeEquivalentTo(_users.Values);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            _memoryCache.TryGetValue($"User_{_user.Id}", out Arg.Any<User>())
                .Returns(callInfo =>
                {
                    callInfo[1] = _user;
                    return true;
                });

            var result = await _userRepository.GetUserByIdAsync(_user.Id);

            result.Should().Be(_user);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _memoryCache.TryGetValue($"User_{userId}", out Arg.Any<User>()).Returns(false);

            Func<Task> act = async () => await _userRepository.GetUserByIdAsync(userId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User {userId} not found");
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUserInCache_WhenUserExists()
        {
            var cacheEntry = Substitute.For<ICacheEntry>();

            _memoryCache.TryGetValue($"User_{_user.Id}", out Arg.Any<User>())
                .Returns(callInfo =>
                {
                    callInfo[1] = _user;
                    return true;
                });

            _memoryCache.CreateEntry($"User_{_user.Id}").Returns(cacheEntry);

            _memoryCache.TryGetValue("UsersKey", out Arg.Any<Dictionary<Guid, User>>())
              .Returns(callInfo =>
              {
                  callInfo[1] = _users;
                  return true;
              });

            var result = await _userRepository.UpdateUserAsync(_user);

            result.Should().Be(_user);
            _memoryCache.Received().Set($"User_{_user.Id}", _user, TimeSpan.FromMinutes(15));
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            _memoryCache.TryGetValue($"User_{_user.Id}", out Arg.Any<User>()).Returns(false);

            Func<Task> act = async () => await _userRepository.UpdateUserAsync(_user);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Cannot update user data. User {_user.Id} not found");
        }
    }
}
