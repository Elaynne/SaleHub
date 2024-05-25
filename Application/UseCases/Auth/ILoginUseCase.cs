using Domain.Models;
using MediatR;

namespace Application.UseCases.Auth
{
    public interface ILoginUseCase : IRequestHandler<LoginInput, string?>
    {
    }
}
