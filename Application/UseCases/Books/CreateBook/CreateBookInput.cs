using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Books.CreateBook
{
    public class CreateBookInput : IRequest<Book>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
    }
}
