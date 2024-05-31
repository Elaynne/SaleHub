using Application.UseCases.Users.RetrieveUserById;
using Domain.Enums;
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

        public async Task<IEnumerable<RetrieveUserByIdOutput>> Handle(RetrieveAllUsersInput request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllUsersAsync();
            var response = new List<RetrieveUserByIdOutput>();

            foreach (var user in allUsers)
            {
                response.Add(new RetrieveUserByIdOutput()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    Active = user.Active
                });
            }
            return request.Role switch
            {
                UserRole.Admin => response,
                UserRole.Seller => GetActiveClients(response),
                _ => new List<RetrieveUserByIdOutput>()
            };
        }
        private IEnumerable<RetrieveUserByIdOutput> GetActiveClients(List<RetrieveUserByIdOutput> users) =>
            users.Where(x => x.Role == UserRole.Client && x.Active == true);
    }
}
