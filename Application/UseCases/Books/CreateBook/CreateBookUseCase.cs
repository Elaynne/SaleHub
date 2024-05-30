using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Books.CreateBook
{
    public class CreateBookUseCase : ICreateBookUseCase
    {
        private readonly IBookRepository _bookRepository;

        public CreateBookUseCase(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> Handle(CreateBookInput request, CancellationToken cancellationToken)
        {
            var book = new Book()
            { 
                Title = request.Title,
                Author = request.Author,
                Isbn = request.ISBN,
                Id = Guid.NewGuid(),
                Description = request.Description,
                Stock = request.Stock,
                Price = request.Price,
                CostPrice = request.CostPrice,
            };
            return await _bookRepository.AddBookAsync(book);
        }
    }
}
