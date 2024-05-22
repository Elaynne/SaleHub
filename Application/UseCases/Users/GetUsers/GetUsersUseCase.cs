
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
            return await _userRepository.GetAllUsersAsync();
        }

    }
}
