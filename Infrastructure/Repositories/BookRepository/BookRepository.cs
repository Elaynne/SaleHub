using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Common;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories.BookRepository;
public class BookRepository : BaseRepository, IBookRepository
{
    private readonly IMemoryCache _memoryCache;
    private const int ExpirationTimeInMinutes = 60;
    public BookRepository(IMemoryCache memoryCache) : base(memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public async Task<Book> AddBookAsync(Book bookInput)
    {
        if (_memoryCache.TryGetValue($"Book_{bookInput.Id}", out Book cachedBook))
        {
            return cachedBook;
        }

        _memoryCache.Set($"Book_{bookInput.Id}", bookInput, TimeSpan.FromMinutes(ExpirationTimeInMinutes));

        AppendDataOnCache<Book>(bookInput, CacheKeys.BooksKey, bookInput.Id, ExpirationTimeInMinutes);

        return bookInput;
    }
    public async Task<List<Book>> GetAllBooksAsync()
    {
        var books = GetDataSet<Book>(CacheKeys.BooksKey);
        return books != null ? books.Values.ToList() : new List<Book>();
    }

    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        if (_memoryCache.TryGetValue($"Book_{id}", out Book cachedBook))
        {
            return cachedBook;
        }
        throw new NotFoundException($"Book {id} not found");
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        if (_memoryCache.TryGetValue($"Book_{book.Id}", out Book cachedBook))
        {
            _memoryCache.Set($"Book_{book.Id}", book, TimeSpan.FromMinutes(ExpirationTimeInMinutes));

            UpdateDataOnCache<Book>(book, book.Id, CacheKeys.BooksKey, ExpirationTimeInMinutes);
            return book;
        }
        throw new NotFoundException($"Cannot update Book data. Book {book.Id} not found");
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var book = await GetBookByIdAsync(id);
        if (book is not null)
        {
            _memoryCache.Remove($"Book_{book.Id}");
            DeleteItemOnCache<Book>(id, CacheKeys.BooksKey, ExpirationTimeInMinutes);
            return true;
        }
        return false;
    }

}
