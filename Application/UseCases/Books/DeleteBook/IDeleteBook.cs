using MediatR;

namespace Application.UseCases.Books.DeleteBook
{
    public interface IDeleteBook : IRequestHandler<DeleteBookInput, bool>
    {
    }
}
