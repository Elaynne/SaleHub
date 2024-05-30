
using Domain.Models;
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

        public async Task<User> Handle(UpdateUserInput request, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateUserAsync(request.User);
        }

    }
}
