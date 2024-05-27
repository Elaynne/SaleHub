using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.GetUsers
{
    public class GetUsersUseCase : IGetUsersUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersInput request, CancellationToken cancellationToken)
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
