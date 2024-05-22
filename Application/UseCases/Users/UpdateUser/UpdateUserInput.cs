using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.UpdateUser
{
    public class UpdateUserInput : IRequest<User>
    {
        public User User { get; set; }
    }
}
