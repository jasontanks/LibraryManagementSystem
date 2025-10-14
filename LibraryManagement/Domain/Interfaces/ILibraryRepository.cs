using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces;

public interface ILibraryRepository
{
    Task<Library?> GetByIdAsync(Guid id);
    Task<IEnumerable<Library>> GetAllAsync();
    Task AddAsync(Library library);
    void Delete(Library library);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> SaveChangesAsync();
}