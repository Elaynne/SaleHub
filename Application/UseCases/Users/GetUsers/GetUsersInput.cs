using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.GetUsers
{
    public class GetUsersInput : IRequest<IEnumerable<User>>
    {
        public Guid? SellerId { get; set; }
    }
}
