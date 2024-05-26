using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.GetUser
{
    public class GetUserInput : IRequest<User>
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
    }
}
