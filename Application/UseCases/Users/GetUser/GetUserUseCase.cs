
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Users.GetUser
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(GetUserInput request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);

            return request.Role switch
            {
                UserRole.Admin => user,
                UserRole.Seller => IsActiveClient(user) ? user : null,
                _ => null
            };
        }
        private bool IsActiveClient(User user) =>
            user.Role == UserRole.Client && user.Active == true;
    }
}
