using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.GetBooks
{
    public interface IGetBooksUseCase : IRequestHandler<GetBooksInput, IEnumerable<Book>>
    {
    }
}
