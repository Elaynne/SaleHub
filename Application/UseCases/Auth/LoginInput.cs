using Domain.Models;
using MediatR;

namespace Application.UseCases.Auth
{
    public class LoginInput: IRequest<string?>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
