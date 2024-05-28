using Domain.Repository.Interfaces;

namespace Application.UseCases.Books.DeleteBook
{
    public class DeleteBookUseCase : IDeleteBookUseCase
    {

        private readonly IBookRepository _bookRepository;
        public DeleteBookUseCase(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<bool> Handle(DeleteBookInput request, CancellationToken cancellationToken)
        {
            return await _bookRepository.DeleteBookAsync(request.Id);
        }
    }
}
