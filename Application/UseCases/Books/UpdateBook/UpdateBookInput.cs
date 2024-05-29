using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.UpdateBook
{
    public class UpdateBookInput : IRequest<Book>
    {
        public Book Book { get; set; }
    }
}
