using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.RetrieveBookById
{
    public class RetrieveBookByIdInput : IRequest<Book>
    {
        public Guid UserId { get; set; }

        public Guid BookId { get; set; }
    }
}
