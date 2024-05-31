using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.RetrieveAllBooks
{
    public class RetrieveAllBooksInput : IRequest<IEnumerable<Book>>
    {
        public Guid UserId { get; set; }
        public UserRole UserRole { get; set; }
    }
}
