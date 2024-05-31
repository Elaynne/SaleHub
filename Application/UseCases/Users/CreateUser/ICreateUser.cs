using Application.UseCases.Users.RetrieveUserById;
using MediatR;

namespace Application.UseCases.Users.CreateUser
{
    public interface ICreateUser : IRequestHandler<CreateUserInput, RetrieveUserByIdOutput>
    {
    }
}
