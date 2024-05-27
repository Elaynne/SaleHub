﻿using Domain.Models;
using Domain.Repository.Interfaces;
using NSubstitute;
using FluentAssertions;
using Application.UseCases.Users.UpdateUser;
using NSubstitute.ExceptionExtensions;

namespace UnitTests.Application.UseCases.Users
{
    public class UpdateUserUseCaseTests
    {
        private IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private User _user = new User { Id = Guid.NewGuid(), UserName = "ElaynneT" };
        private UpdateUserInput _request = new UpdateUserInput();
        public UpdateUserUseCaseTests()
        {
            _request = new UpdateUserInput { User = _user };
        }
        [Fact]
        public async Task Handle_ReturnsUpdatedUser()
        {
            _userRepository.UpdateUserAsync(Arg.Any<User>()).Returns(_user);
            var updateUserUseCase = new UpdateUserUseCase(_userRepository);

            var result = await updateUserUseCase.Handle(_request, CancellationToken.None);

            result.Should().BeEquivalentTo(_user);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserRepositoryThrows()
        {
            _userRepository.UpdateUserAsync(Arg.Any<User>()).Throws(new Exception("Repository exception"));
            var updateUserUseCase = new UpdateUserUseCase(_userRepository);

            Func<Task> act = async () => await updateUserUseCase.Handle(_request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Repository exception");
        }

    }
}
