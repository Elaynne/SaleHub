using Application.UseCases.Users.RetrieveUserById;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.UpdateUser
{
    public class UpdateUserInput : IRequest<RetrieveUserByIdOutput>
    {
        public User User { get; set; }
    }
}
