using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Books.RetrieveBookById
{
    public class RetrieveBookById : IRetrieveBookById
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<RetrieveBookById> _logger;
        public RetrieveBookById(IBookRepository bookRepository,
            ILogger<RetrieveBookById> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<Book?> Handle(RetrieveBookByIdInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting book {BookId} for user {UserId}", request.BookId, request.UserId);

            return await _bookRepository.GetBookByIdAsync(request.BookId);
        }
    }
}
