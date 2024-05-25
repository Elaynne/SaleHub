
using Domain.Models;

namespace Domain.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
