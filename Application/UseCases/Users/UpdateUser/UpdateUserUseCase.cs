
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.UpdateUser
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(UpdateUserInput request, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateUserAsync(request.User);
        }

    }
}
