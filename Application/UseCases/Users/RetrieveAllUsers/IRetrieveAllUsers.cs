using Application.UseCases.Users.RetrieveUserById;
using MediatR;

namespace Application.UseCases.Users.RetrieveAllUsers
{
    public interface IRetrieveAllUsers : IRequestHandler<RetrieveAllUsersInput, IEnumerable<RetrieveUserByIdOutput>>
    {
    }
}
