using Domain.Cache;
using Domain.Repository.Interfaces;
using Infrastructure.Common;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repository.User;
public class UserRepository : BaseRepository, IUserRepository
{
    private readonly IMemoryCache _memoryCache;
    private const int ExpirationTimeInMinutes = 60;
    public UserRepository(IMemoryCache memoryCache) : base(memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public async Task<Domain.Models.User> AddUserAsync(Domain.Models.User userInput)
    {
        if (_memoryCache.TryGetValue($"User_{userInput.Id}", out Domain.Models.User cachedUser))
        {
            return cachedUser;
        }

        _memoryCache.Set($"User_{userInput.Id}", userInput, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
       
        AppendDataOnCache(userInput, CacheKeys.UsersKey, userInput.Id, ExpirationTimeInMinutes);

        return userInput;
    }
    public async Task<List<Domain.Models.User>> GetAllUsersAsync()
    {
        var users = GetDataSet<Domain.Models.User>(CacheKeys.UsersKey);
        return (users != null) ? users.Values.ToList() : new List<Domain.Models.User>();
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
            _memoryCache.Set($"User_{user.Id}", user, TimeSpan.FromMinutes(ExpirationTimeInMinutes));

            UpdateDataOnCache<Domain.Models.User>(user, user.Id, CacheKeys.UsersKey, ExpirationTimeInMinutes);
            return user;
        }
        throw new NotFoundException($"Cannot update user data. User {user.Id} not found");
    }
    
   
}
