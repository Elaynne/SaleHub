using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.GetUser
{
    public interface IGetUserUseCase : IRequestHandler<GetUserInput, User>
    {
    }
}
