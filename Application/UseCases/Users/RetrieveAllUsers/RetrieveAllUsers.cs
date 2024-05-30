using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.RetrieveAllUsers
{
    public class RetrieveAllUsers : IRetrieveAllUsers
    {
        private readonly IUserRepository _userRepository;

        public RetrieveAllUsers(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(RetrieveAllUsersInput request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllUsersAsync();

            return request.Role switch
            {
                UserRole.Admin => allUsers,
                UserRole.Seller => GetActiveClients(allUsers),
                _ => new List<User>()
            };
        }
        private IEnumerable<User> GetActiveClients(List<User> users) =>
            users.Where(x => x.Role == UserRole.Client && x.Active == true);
    }
}
