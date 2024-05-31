using Application.UseCases.Users.RetrieveUserById;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Users.RetrieveAllUsers
{
    public class RetrieveAllUsersInput : IRequest<IEnumerable<RetrieveUserByIdOutput>>
    {
        public UserRole Role { get; set; }
    }
}
