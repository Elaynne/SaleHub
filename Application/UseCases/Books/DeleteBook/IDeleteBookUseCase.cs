using MediatR;

namespace Application.UseCases.Books.DeleteBook
{
    public interface IDeleteBookUseCase : IRequestHandler<DeleteBookInput, bool>
    {
    }
}
