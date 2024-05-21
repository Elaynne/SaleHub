using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.CreateUser
{
    public interface ICreateUserUseCase : IRequestHandler<CreateUserInput, User>
    {
    }
}
