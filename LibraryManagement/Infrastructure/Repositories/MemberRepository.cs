using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<MemberRepository> _logger;

    public MemberRepository(AppDbContext context, ILogger<MemberRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Member?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in MemberRepository while getting member with ID {MemberId}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<Member> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            var totalCount = await _context.Members.CountAsync();
            var items = await _context.Members
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in MemberRepository while getting all members");
            throw;
        }
    }

    public async Task<IEnumerable<Member>> GetByLibraryIdAsync(Guid libraryId)
    {
        try
        {
            return await _context.Members
                .Where(m => m.LibraryId == libraryId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in MemberRepository while getting members for LibraryId {LibraryId}", libraryId);
            throw;
        }
    }

    public async Task AddAsync(Member member)
    {
        try
        {
            await _context.Members.AddAsync(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in MemberRepository while adding a member with Name '{MemberName}'", member.FullName);
            throw;
        }
    }

    public void Delete(Member member)
    {
        try
        {
            _context.Members.Remove(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in MemberRepository while deleting a member with ID {MemberId}", member.Id);
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
            _logger.LogError(ex, "An error occurred in MemberRepository while saving changes to the database.");
            throw;
        }
    }
}