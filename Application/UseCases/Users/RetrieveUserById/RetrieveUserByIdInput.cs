using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Users.RetrieveUserById
{
    public class RetrieveUserByIdInput : IRequest<RetrieveUserByIdOutput>
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
    }
}
