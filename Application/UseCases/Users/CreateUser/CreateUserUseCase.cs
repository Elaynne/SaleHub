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
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName= request.UserName,
                Email = request.Email,
                Password = request.Password,
                Active = true,
                Role = request.Role
            };
            
            return await _userRepository.AddUserAsync(user);
        }
    }

}