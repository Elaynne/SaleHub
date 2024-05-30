using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.CreateUser
{
    public interface ICreateUser : IRequestHandler<CreateUserInput, User>
    {
    }
}
