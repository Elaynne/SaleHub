
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Users.RetrieveUserById
{
    public class RetrieveUserById : IRetrieveUserById
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RetrieveUserById> _logger;
        public RetrieveUserById(IUserRepository userRepository,
            ILogger<RetrieveUserById> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User?> Handle(RetrieveUserByIdInput request, CancellationToken cancellationToken)
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
