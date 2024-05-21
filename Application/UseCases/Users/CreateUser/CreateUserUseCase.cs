using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.CreateUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public CreateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(CreateUserInput request, CancellationToken cancellationToken)
        {
            return await _userRepository.AddUserAsync(request.User);
        }
    }

}