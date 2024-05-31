
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

        public async Task<RetrieveUserByIdOutput?> Handle(RetrieveUserByIdInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting user {UserId}", request.Id);

            var user = await _userRepository.GetUserByIdAsync(request.Id);

            var response = new RetrieveUserByIdOutput() { 
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Active = user.Active,
                Role = user.Role

            };
            return request.Role switch
            {
                UserRole.Admin => response,
                UserRole.Seller => IsActiveClient(user) ? response : null,
                _ => null
            };
        }
        private bool IsActiveClient(Domain.Models.User user) =>
            user.Role == UserRole.Client && user.Active == true;
    }
}
