using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.RetrieBookDetails
{
    public class RetrieBookDetailsInput : IRequest<Book>
    {
        public Guid UserId { get; set; }

        public Guid BookId { get; set; }
    }
}
