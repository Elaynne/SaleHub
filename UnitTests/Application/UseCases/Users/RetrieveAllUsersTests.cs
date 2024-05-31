using Application.UseCases.Users.RetrieveAllUsers;
using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using NSubstitute;
using FluentAssertions;
using Application.UseCases.Users.RetrieveUserById;

namespace UnitTests.Application.UseCases.Users
{
    public class RetrieveAllUsersTests
    {
        private IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private RetrieveAllUsers _getUsersUseCase;
        private List<Domain.Models.User> _allUsers = new List<Domain.Models.User>
            {
                new Domain.Models.User { Id = Guid.NewGuid(), Role = UserRole.Admin },
                new Domain.Models.User { Id = Guid.NewGuid(), Role = UserRole.Seller },
                new Domain.Models.User { Id = Guid.NewGuid(), Role = UserRole.Client, Active = true }
            };

        private List<global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput> _allUsersOutput = new List<global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput>();
        public RetrieveAllUsersTests()
        {
            _userRepository.GetAllUsersAsync().Returns(_allUsers);
            foreach (var user in _allUsers)
            {
                _allUsersOutput.Add(new global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput()
                {
                    Id = user.Id,
                    Role = user.Role,
                    Active = user.Active
                });
            };

            _getUsersUseCase = new RetrieveAllUsers(_userRepository);
        }

        [Fact]
        public async Task Handle_AdminRole_ReturnsAllUsers()
        {
            var request = new RetrieveAllUsersInput { Role = UserRole.Admin };

            var result = await _getUsersUseCase.Handle(request, CancellationToken.None);

            result.Should().BeEquivalentTo(_allUsersOutput);
        }

        [Fact]
        public async Task Handle_SellerRole_ReturnsActiveClients()
        {
            var request = new RetrieveAllUsersInput { Role = UserRole.Seller };

            var result = await _getUsersUseCase.Handle(request, CancellationToken.None);

            result.Should().ContainSingle().Which.Should().BeEquivalentTo(_allUsersOutput.Last());
        }

        [Fact]
        public async Task Handle_ClientRole_ReturnsEmptyList()
        {
            var request = new RetrieveAllUsersInput { Role = UserRole.Client }; 

            var result = await _getUsersUseCase.Handle(request, CancellationToken.None);

            result.Should().BeEmpty();
        }
    }
}
