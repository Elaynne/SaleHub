using Application.UseCases.Users.RetrieveUserById;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Users.CreateUser
{
    public class CreateUserInput : IRequest<RetrieveUserByIdOutput>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
