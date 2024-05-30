using Application.UseCases.Books.RetrieBookDetails;
using Domain.Models;
using Domain.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Books.RetrieBookDetails
{
    public class RetrieBookDetails : IRetrieBookDetails
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<RetrieBookDetails> _logger;
        public RetrieBookDetails(IBookRepository bookRepository,
            ILogger<RetrieBookDetails> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<Book?> Handle(RetrieBookDetailsInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting book {BookId} for user {UserId}", request.BookId, request.UserId);

            return await _bookRepository.GetBookByIdAsync(request.BookId);
        }
    }
}
