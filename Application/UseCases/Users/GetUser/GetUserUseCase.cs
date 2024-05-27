
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.GetUser
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserUseCase> _logger;
        public GetUserUseCase(IUserRepository userRepository,
            ILogger<GetUserUseCase> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User?> Handle(GetUserInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting user {UserId}", request.Id);

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
