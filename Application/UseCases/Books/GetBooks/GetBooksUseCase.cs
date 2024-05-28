using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Books.GetBooks
{
    public class GetBooksUseCase : IGetBooksUseCase
    {
        private readonly IBookRepository _bookRepository;

        public GetBooksUseCase(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> Handle(GetBooksInput request, CancellationToken cancellationToken)
        {
            return await _bookRepository.GetAllBooksAsync();
        }
    }
}
