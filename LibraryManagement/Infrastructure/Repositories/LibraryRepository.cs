using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<LibraryRepository> _logger;

    public LibraryRepository(AppDbContext context, ILogger<LibraryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Library?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Libraries
                .Include(l => l.Books)
                .Include(l => l.Members)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in LibraryRepository while getting library with ID {LibraryId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Library>> GetAllAsync()
    {
        try
        {
            return await _context.Libraries.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in LibraryRepository while getting all libraries");
            throw;
        }
    }

    public async Task AddAsync(Library library)
    {
        try
        {
            await _context.Libraries.AddAsync(library);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in LibraryRepository while adding a library with Name '{LibraryName}'", library.Name);
            throw;
        }
    }

    public void Delete(Library library)
    {
        try
        {
            _context.Libraries.Remove(library);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in LibraryRepository while deleting a library with ID {LibraryId}", library.Id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        try
        {
            return await _context.Libraries.AnyAsync(l => l.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in LibraryRepository while checking for existence of library with ID {LibraryId}", id);
            throw;
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in LibraryRepository while saving changes to the database.");
            throw;
        }
    }
}