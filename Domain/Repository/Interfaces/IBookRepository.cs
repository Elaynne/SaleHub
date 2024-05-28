
using Domain.Models;

namespace Domain.Repository.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIdAsync(Guid id);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> AddBookAsync(Book user);
        Task<Book> UpdateBookAsync(Book user);
    }
}
