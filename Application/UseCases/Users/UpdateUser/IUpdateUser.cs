using Application.UseCases.Users.RetrieveUserById;
using MediatR;

namespace Application.UseCases.Users.UpdateUser
{
    public interface IUpdateUser : IRequestHandler<UpdateUserInput, RetrieveUserByIdOutput>
    {
    }
}
