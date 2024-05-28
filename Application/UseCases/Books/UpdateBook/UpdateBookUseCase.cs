using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Books.UpdateBook
{
    public class UpdateBookUseCase : IUpdateBookUseCase
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookUseCase(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> Handle(UpdateBookInput request, CancellationToken cancellationToken)
        {
            return await _bookRepository.UpdateBookAsync(request.Book);
        }
    }
}
