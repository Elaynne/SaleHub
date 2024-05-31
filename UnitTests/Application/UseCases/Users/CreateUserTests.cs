using Domain.Models;
using Domain.Repository.Interfaces;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.RetrieveUserById;

namespace UnitTests.Application.UseCases.Users
{
    public class CreateUserTests
    {
        private IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private CreateUserInput _request = new CreateUserInput();
        public CreateUserTests()
        {
            _request = new CreateUserInput { 
                UserName = "ElaynneT", 
                Email = "elaynne@example.com" ,
                Password = "111"
            };
        }
        [Fact]
        public async Task Handle_ReturnsCreatedUser()
        {
            var user = new User { 
                Id = Guid.NewGuid(), 
                UserName = _request.UserName, 
                Email = _request.Email,
                Password = _request.Password,
                Active = true
            };

            var outPut = new RetrieveUserByIdOutput() { 
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Active = user.Active
            };

            _userRepository.AddUserAsync(Arg.Any<User>()).Returns(user);
            var createUserUseCase = new CreateUser(_userRepository);

            var result = await createUserUseCase.Handle(_request, CancellationToken.None);

            result.Should().BeEquivalentTo(outPut);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserRepositoryThrows()
        {
            _userRepository.AddUserAsync(Arg.Any<User>()).Throws(new Exception("Repository exception"));
            var createUserUseCase = new CreateUser(_userRepository);

            Func<Task> act = async () => await createUserUseCase.Handle(_request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Repository exception");
        }

    }
}
