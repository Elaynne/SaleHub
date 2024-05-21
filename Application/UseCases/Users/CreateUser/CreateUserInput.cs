using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.CreateUser
{
    public class CreateUserInput : IRequest<User>
    {
        public User User { get; set; }
    }
}
