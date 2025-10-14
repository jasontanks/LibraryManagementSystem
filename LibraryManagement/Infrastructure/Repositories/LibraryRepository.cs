using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly AppDbContext _context;

    public LibraryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Library?> GetByIdAsync(Guid id)
    {
        return await _context.Libraries
            .Include(l => l.Books)
            .Include(l => l.Members)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Library>> GetAllAsync()
    {
        return await _context.Libraries.ToListAsync();
    }

    public async Task AddAsync(Library library)
    {
        await _context.Libraries.AddAsync(library);
    }

    public void Delete(Library library)
    {
        _context.Libraries.Remove(library);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Libraries.AnyAsync(l => l.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}