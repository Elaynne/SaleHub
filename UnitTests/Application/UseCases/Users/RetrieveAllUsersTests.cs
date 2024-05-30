using Application.UseCases.Users.RetrieveAllUsers;
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using NSubstitute;
using FluentAssertions;

namespace UnitTests.Application.UseCases.Users
{
    public class RetrieveAllUsersTests
    {
        private IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private RetrieveAllUsers _retrieveAllUsersUseCase;
        private List<User> _allUsers = new List<User>
            {
                new User { Id = Guid.NewGuid(), Role = UserRole.Admin },
                new User { Id = Guid.NewGuid(), Role = UserRole.Seller },
                new User { Id = Guid.NewGuid(), Role = UserRole.Client, Active = true }
            };
        public RetrieveAllUsersTests()
        {
            _userRepository.GetAllUsersAsync().Returns(_allUsers);
            _retrieveAllUsersUseCase = new RetrieveAllUsers(_userRepository);
        }

        [Fact]
        public async Task Handle_AdminRole_ReturnsAllUsers()
        {
            var request = new RetrieveAllUsersInput { Role = UserRole.Admin };

            var result = await _retrieveAllUsersUseCase.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(_allUsers);
        }

        [Fact]
        public async Task Handle_SellerRole_ReturnsActiveClients()
        {
            var request = new RetrieveAllUsersInput { Role = UserRole.Seller };

            var result = await _retrieveAllUsersUseCase.Handle(request, CancellationToken.None);

            result.Should().ContainSingle().Which.Should().BeEquivalentTo(_allUsers.Last());
        }

        [Fact]
        public async Task Handle_ClientRole_ReturnsEmptyList()
        {
            var request = new RetrieveAllUsersInput { Role = UserRole.Client }; 

            var result = await _retrieveAllUsersUseCase.Handle(request, CancellationToken.None);

            result.Should().BeEmpty();
        }
    }
}
