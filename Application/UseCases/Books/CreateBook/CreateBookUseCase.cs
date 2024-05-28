using AutoMapper;
using Domain.Models;
using Domain.Repository.Interfaces;

namespace Application.UseCases.Books.CreateBook
{
    public class CreateBookUseCase : ICreateBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public CreateBookUseCase(IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<Book> Handle(CreateBookInput request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);
            return await _bookRepository.AddBookAsync(book);
        }
    }
}
