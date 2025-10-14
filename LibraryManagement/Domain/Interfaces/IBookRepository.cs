using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Book> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
    Task<(IEnumerable<Book> Items, int TotalCount)> SearchAsync(string searchTerm, int pageNumber, int pageSize);
    Task<IEnumerable<Book>> GetByLibraryIdAsync(Guid libraryId);
    Task AddAsync(Book book);
    void Delete(Book book);
    Task<bool> SaveChangesAsync();
}