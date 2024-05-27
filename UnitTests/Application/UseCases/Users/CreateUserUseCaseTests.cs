using Domain.Models;
using Domain.Repository.Interfaces;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Application.UseCases.Users.CreateUser;
using AutoMapper;

namespace UnitTests.Application.UseCases.Users
{
    public class CreateUserUseCaseTests
    {
        private IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private IMapper _mapper = Substitute.For<IMapper>();
        private CreateUserInput _request = new CreateUserInput();
        public CreateUserUseCaseTests()
        {
            _request = new CreateUserInput { 
                UserName = "ElaynneT", 
                Email = "elaynne@example.com" 
            };
        }
        [Fact]
        public async Task Handle_ReturnsCreatedUser()
        {
            var user = new User { 
                Id = Guid.NewGuid(), 
                UserName = _request.UserName, 
                Email = _request.Email 
            };

            _userRepository.AddUserAsync(Arg.Any<User>()).Returns(user);
            _mapper.Map<User>(Arg.Any<CreateUserInput>()).Returns(user);
            var createUserUseCase = new CreateUserUseCase(_userRepository, _mapper);

            var result = await createUserUseCase.Handle(_request, CancellationToken.None);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserRepositoryThrows()
        {
            _userRepository.AddUserAsync(Arg.Any<User>()).Throws(new Exception("Repository exception"));
            var createUserUseCase = new CreateUserUseCase(_userRepository, _mapper);

            Func<Task> act = async () => await createUserUseCase.Handle(_request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Repository exception");
        }

    }
}
