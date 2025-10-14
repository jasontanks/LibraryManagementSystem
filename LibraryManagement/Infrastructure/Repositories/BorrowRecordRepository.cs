using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly AppDbContext _context;

    public BorrowRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BorrowRecord borrowRecord)
    {
        await _context.BorrowRecords.AddAsync(borrowRecord);
    }

    public async Task<BorrowRecord?> GetByIdAsync(Guid borrowId)
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .FirstOrDefaultAsync(br => br.Id == borrowId);
    }

    public async Task<BorrowRecord?> GetActiveBorrowRecordForBookAsync(Guid bookId)
    {
        return await _context.BorrowRecords.FirstOrDefaultAsync(br => br.BookId == bookId && br.ReturnedAt == null);
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowedBooksByMemberAsync(Guid memberId)
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .Where(br => br.MemberId == memberId && br.ReturnedAt == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByMemberAsync(Guid memberId)
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .Where(br => br.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByBookAsync(Guid bookId)
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)
            .Include(br => br.Member)
            .Where(br => br.BookId == bookId)
            .ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}