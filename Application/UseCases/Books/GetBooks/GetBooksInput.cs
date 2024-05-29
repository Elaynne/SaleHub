using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.GetBooks
{
    public class GetBooksInput : IRequest<IEnumerable<Book>>
    {
        public Guid UserId { get; set; }
    }
}
