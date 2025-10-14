using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _context;

    public MemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Member?> GetByIdAsync(Guid id)
    {
        return await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<(IEnumerable<Member> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Members.CountAsync();
        var items = await _context.Members
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<IEnumerable<Member>> GetByLibraryIdAsync(Guid libraryId)
    {
        return await _context.Members
            .Where(m => m.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task AddAsync(Member member)
    {
        await _context.Members.AddAsync(member);
    }

    public void Delete(Member member)
    {
        _context.Members.Remove(member);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}