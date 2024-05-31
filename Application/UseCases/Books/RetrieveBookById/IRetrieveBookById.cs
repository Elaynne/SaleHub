using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.RetrieveBookById
{
    public interface IRetrieveBookById : IRequestHandler<RetrieveBookByIdInput, Book>
    {
    }
}
