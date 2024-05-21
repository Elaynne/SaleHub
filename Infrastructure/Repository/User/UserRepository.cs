using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repository.User;
public class UserRepository : IUserRepository
{
    private readonly IMemoryCache _memoryCache;
    private const string UserIdsCacheKey = "UserIds";
    public UserRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public async Task<Domain.Models.User> AddUserAsync(Domain.Models.User userInput)
    {
        if (_memoryCache.TryGetValue($"User_{userInput.Id}", out Domain.Models.User cachedUser))
        {
            return cachedUser;
        }

        _memoryCache.Set($"User_{userInput.Id}", userInput, TimeSpan.FromMinutes(5));
       // _memoryCache.TryGetValue(UserIdsCacheKey,);
        return userInput;
    }

    public Task<IEnumerable<Domain.Models.User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
        //if (_memoryCache.get($"User_{user.Id}", out Domain.Models.User cachedUser))
        //{
        //    _memoryCache.Set($"User_{user.Id}", user, TimeSpan.FromMinutes(5));
        //    return user;
        //}
    }

    public async Task<Domain.Models.User> GetUserByIdAsync(Guid id)
    {
        if (_memoryCache.TryGetValue($"User_{id}", out Domain.Models.User cachedUser))
        {
            return cachedUser;
        }
        throw new NotFoundException($"User {id} not found");
    }

    public async Task<Domain.Models.User> UpdateUserAsync(Domain.Models.User user)
    {
        if (_memoryCache.TryGetValue($"User_{user.Id}", out Domain.Models.User cachedUser))
        {
            _memoryCache.Set($"User_{user.Id}", user, TimeSpan.FromMinutes(5));
            return user;
        }
        throw new NotFoundException($"Cannot update user data. User {user.Id} not found");
    }

    public Task<bool> DeleteUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
