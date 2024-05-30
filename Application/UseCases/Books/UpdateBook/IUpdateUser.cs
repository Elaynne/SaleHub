using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.UpdateBook
{
    public interface IUpdateBook : IRequestHandler<UpdateBookInput, Book>
    {
    }
}
