using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<BookRepository> _logger;

    public BookRepository(AppDbContext context, ILogger<BookRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BookRepository while getting book with ID {BookId}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<Book> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BookRepository while searching for books with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    public async Task<(IEnumerable<Book> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            var totalCount = await _context.Books.CountAsync();
            var items = await _context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BookRepository while getting all books");
            throw;
        }
    }

    public async Task<IEnumerable<Book>> GetByLibraryIdAsync(Guid libraryId)
    {
        try
        {
            return await _context.Books
                .Where(b => b.LibraryId == libraryId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BookRepository while getting books for LibraryId {LibraryId}", libraryId);
            throw;
        }
    }

    public async Task AddAsync(Book book)
    {
        try
        {
            await _context.Books.AddAsync(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BookRepository while adding a book with Title '{BookTitle}'", book.Title);
            throw;
        }
    }

    public void Delete(Book book)
    {
        try
        {
            _context.Books.Remove(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BookRepository while deleting a book with ID {BookId}", book.Id);
            throw;
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "A database update error occurred while saving changes. This might be due to a constraint violation.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in BookRepository while saving changes to the database.");
            throw;
        }
    }
}