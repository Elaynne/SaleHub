using Domain.Repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repository.User;
public class UserRepository : IUserRepository
{
    private readonly IMemoryCache _memoryCache;
    public UserRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public async Task<Domain.Models.User> AddUserAsync(Domain.Models.User userInput)
    {
        Domain.Models.User cachedUser = new Domain.Models.User();

        if (_memoryCache.TryGetValue($"User_{userInput.Id}", out cachedUser))
        {
            return cachedUser;
        }

        _memoryCache.Set($"User_{userInput.Id}", userInput, TimeSpan.FromMinutes(5));
        return userInput;
    }

    public Task<IEnumerable<Domain.Models.User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Models.User> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(Domain.Models.User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}
