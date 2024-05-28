using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.GetBook
{
    public class GetBookInput : IRequest<Book>
    {
        public Guid Id { get; set; }
    }
}
