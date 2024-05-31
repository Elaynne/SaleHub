using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Books.RetrieveAllBooks
{
    public class RetrieveAllBooks : IRetrieveAllBooks
    {
        private readonly IBookRepository _bookRepository;

        public RetrieveAllBooks(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> Handle(RetrieveAllBooksInput request, CancellationToken cancellationToken)
        {
            return await _bookRepository.GetAllBooksAsync();
        }
    }
}
