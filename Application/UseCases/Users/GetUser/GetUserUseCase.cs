
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.GetUser
{
    public class GetUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserInput request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByIdAsync(request.User.Id);
        }
    }
}
