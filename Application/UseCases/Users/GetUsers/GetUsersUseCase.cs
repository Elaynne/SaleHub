
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using MediatR;

namespace Application.UseCases.Users.GetUsers
{
    public class GetUsersUseCase : IGetUsersUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersInput request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllUsersAsync();

            if (request.Role == UserRole.Admin)
            {
                return allUsers;
            }
            return GetCustomersBySeller(request.SellerId, allUsers);
        }

        private IEnumerable<User> GetCustomersBySeller(Guid? sellerId, List<User> users)
        {
            return users.Where(x => x.SellerId == sellerId).ToList();
        }

    }
}
