
using Application.UseCases.Users.Encryption;
using Application.UseCases.Users.RetrieveUserById;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.UpdateUser
{
    public class UpdateUser : IUpdateUser
    {
        private readonly IUserRepository _userRepository;

        public UpdateUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RetrieveUserByIdOutput> Handle(UpdateUserInput request, CancellationToken cancellationToken)
        {
            request.User.Password = Encrypt.HashPassword(request.User.Password);
            var response = await _userRepository.UpdateUserAsync(request.User);

            return new RetrieveUserByIdOutput()
            { 
                Id = response.Id,
                UserName = response.UserName,
                Email = response.Email,
                Role = response.Role,
                Active = response.Active
            };
        }

    }
}
