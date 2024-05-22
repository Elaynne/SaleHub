using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.UpdateUser
{
    public interface IUpdateUserUseCase : IRequestHandler<UpdateUserInput, User>
    {
    }
}
