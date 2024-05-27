using Domain.Enums;
using Domain.Models;
using Domain.Repository.Interfaces;
using NSubstitute;
using FluentAssertions;
using Application.UseCases.Users.GetUser;
using Microsoft.Extensions.Logging;

namespace UnitTests.Application.UseCases.Users
{
    public class GetUserUseCaseTests
    {
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private GetUserUseCase _getUserUseCase;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly ILogger<GetUserUseCase> _logger = Substitute.For<ILogger<GetUserUseCase>>();
        public GetUserUseCaseTests()
        {
            _getUserUseCase = new GetUserUseCase(_userRepository, _logger);
        }
        [Fact]
        public async Task Handle_AdminRole_ReturnsUser()
        {
            var user = new User { Id = _userId, Role = UserRole.Admin };
            _userRepository.GetUserByIdAsync(_userId).Returns(user);

            var request = new GetUserInput { Id = _userId, Role = UserRole.Admin };

            var result = await _getUserUseCase.Handle(request, CancellationToken.None);

            result.Should().Be(user);
        }

        [Fact]
        public async Task Handle_SellerRoleWithActiveClient_ReturnsUser()
        {
            var user = new User { Id = _userId, Role = UserRole.Client, Active = true };
            _userRepository.GetUserByIdAsync(_userId).Returns(user);

            var request = new GetUserInput { Id = _userId, Role = UserRole.Seller };

            var result = await _getUserUseCase.Handle(request, CancellationToken.None);

            result.Should().Be(user);
        }

        [Fact]
        public async Task Handle_SellerRoleWithInactiveClient_ReturnsNull()
        {
            var user = new User { Id = _userId, Role = UserRole.Client, Active = false };
            _userRepository.GetUserByIdAsync(_userId).Returns(user);

            var request = new GetUserInput { Id = _userId, Role = UserRole.Seller };

            var result = await _getUserUseCase.Handle(request, CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ClientRole_ReturnsNull()
        {
            var user = new User { Id = _userId, Role = UserRole.Client, Active = true };
            _userRepository.GetUserByIdAsync(_userId).Returns(user);

            var request = new GetUserInput { Id = _userId, Role = UserRole.Client }; 

            var result = await _getUserUseCase.Handle(request, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
