using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.RetrieBookDetails
{
    public interface IRetrieBookDetails : IRequestHandler<RetrieBookDetailsInput, Book>
    {
    }
}
