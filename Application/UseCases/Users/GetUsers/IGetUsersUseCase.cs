using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.GetUsers
{
    public interface IGetUsersUseCase : IRequestHandler<GetUsersInput, IEnumerable<User>>
    {
    }
}
