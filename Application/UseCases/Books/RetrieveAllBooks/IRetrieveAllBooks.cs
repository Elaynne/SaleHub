using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.RetrieveAllBooks
{
    public interface IRetrieveAllBooks : IRequestHandler<RetrieveAllBooksInput, IEnumerable<Book>>
    {
    }
}
