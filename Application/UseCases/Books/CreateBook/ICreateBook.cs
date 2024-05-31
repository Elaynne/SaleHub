using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.CreateBook
{
    public interface ICreateBook : IRequestHandler<CreateBookInput, Book>
    {
    }
}
