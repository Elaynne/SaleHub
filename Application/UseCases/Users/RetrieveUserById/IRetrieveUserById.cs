using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.RetrieveUserById
{
    public interface IRetrieveUserById : IRequestHandler<RetrieveUserByIdInput, User>
    {
    }
}
