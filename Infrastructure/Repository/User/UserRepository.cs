using Domain.Enums;
using Domain.Repository.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repository.User;
public class UserRepository : IUserRepository
{
    private readonly IMemoryCache _memoryCache;
    private const string UsersCacheKey = "UsersKey";
    private const int ExpirationTimeInMinutes = 15;
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

        _memoryCache.Set($"User_{userInput.Id}", userInput, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
       
        AppendUserOnUsersDB(userInput);

        return userInput;
    }
    public async Task<List<Domain.Models.User>> GetAllUsersAsync()
    {
        var users = GetUsers();
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

            UpdateUserOnUsersDB(user);
            return user;
        }
        throw new NotFoundException($"Cannot update user data. User {user.Id} not found");
    }
    private void AppendUserOnUsersDB(Domain.Models.User userInput)
    {
        if (!_memoryCache.TryGetValue(UsersCacheKey, out Dictionary<Guid, Domain.Models.User> users))
            users = new Dictionary<Guid, Domain.Models.User>();
       
        users.Add(userInput.Id, userInput);
        
        _memoryCache.Set(UsersCacheKey, users, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
    }
    public void UpdateUserOnUsersDB(Domain.Models.User user)
    {
        var users = GetUsers();
        users[user.Id] = user;

        _memoryCache.Set(UsersCacheKey, users, TimeSpan.FromMinutes(ExpirationTimeInMinutes));
    }
    private Dictionary<Guid, Domain.Models.User> GetUsers()
    {
        _memoryCache.TryGetValue(UsersCacheKey, out Dictionary<Guid, Domain.Models.User> cachedUsers);

        return cachedUsers;
    }
   
}
