using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.RetrieveAllUsers
{
    public class RetrieveAllUsersInput : IRequest<IEnumerable<User>>
    {
        public UserRole Role { get; set; }
    }
}
