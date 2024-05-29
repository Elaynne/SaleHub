using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.UpdateBook
{
    public interface IUpdateBookUseCase : IRequestHandler<UpdateBookInput, Book>
    {
    }
}
