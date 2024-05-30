using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.UpdateUser
{
    public interface IUpdateUser : IRequestHandler<UpdateUserInput, User>
    {
    }
}
