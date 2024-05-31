using Application.UseCases.Users.Encryption;
using Application.UseCases.Users.RetrieveUserById;
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.CreateUser
{
    public class CreateUser : ICreateUser
    {
        private readonly IUserRepository _userRepository;

        public CreateUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RetrieveUserByIdOutput> Handle(CreateUserInput request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName= request.UserName,
                Email = request.Email,
                Password = Encrypt.HashPassword(request.Password),
                Active = true,
                Role = request.Role
            };
            
            var response = await _userRepository.AddUserAsync(user);
            return new RetrieveUserByIdOutput()
            {
                Id = response.Id,
                UserName = response.UserName,
                Email = response.Email,
                Active = true,
                Role = response.Role
            };
        }
    }

}