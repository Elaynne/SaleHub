using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.CreateBook
{
    public interface ICreateBookUseCase : IRequestHandler<CreateBookInput, Book>
    {
    }
}
