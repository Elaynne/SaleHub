using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.CreateUser
{
    public class CreateUserInput : IRequest<User>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
