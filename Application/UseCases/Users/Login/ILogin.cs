using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.Login
{
    public interface ILogin : IRequestHandler<LoginInput, string?>
    {
    }
}
