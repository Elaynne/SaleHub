using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.GetBook
{
    public interface IGetBookUseCase : IRequestHandler<GetBookInput, Book>
    {
    }
}
