using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.DeleteBook
{
    public class DeleteBookInput : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
