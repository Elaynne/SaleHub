using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.RetrieveAllUsers
{
    public interface IRetrieveAllUsers : IRequestHandler<RetrieveAllUsersInput, IEnumerable<User>>
    {
    }
}
