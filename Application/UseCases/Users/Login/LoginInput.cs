using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.Login
{
    public class LoginInput : IRequest<string?>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
