using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Books.GetBook
{
    public class GetBookUseCase : IGetBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<GetBookUseCase> _logger;
        public GetBookUseCase(IBookRepository bookRepository,
            ILogger<GetBookUseCase> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<Book?> Handle(GetBookInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting book {UserId}", request.Id);

            return await _bookRepository.GetBookByIdAsync(request.Id);
        }
    }
}
