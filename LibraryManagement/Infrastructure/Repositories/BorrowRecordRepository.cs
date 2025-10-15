using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<BorrowRecordRepository> _logger;

    public BorrowRecordRepository(AppDbContext context, ILogger<BorrowRecordRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(BorrowRecord borrowRecord)
    {
        try
        {
            await _context.BorrowRecords.AddAsync(borrowRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while adding a borrow record for BookId {BookId}", borrowRecord.BookId);
            throw;
        }
    }

    public async Task<BorrowRecord?> GetByIdAsync(Guid borrowId)
    {
        try
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .FirstOrDefaultAsync(br => br.Id == borrowId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while getting borrow record with ID {BorrowId}", borrowId);
            throw;
        }
    }

    public async Task<BorrowRecord?> GetActiveBorrowRecordForBookAsync(Guid bookId)
    {
        try
        {
            return await _context.BorrowRecords.FirstOrDefaultAsync(br => br.BookId == bookId && br.ReturnedAt == null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while getting active borrow record for BookId {BookId}", bookId);
            throw;
        }
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowedBooksByMemberAsync(Guid memberId)
    {
        try
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => br.MemberId == memberId && br.ReturnedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while getting borrowed books for MemberId {MemberId}", memberId);
            throw;
        }
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByMemberAsync(Guid memberId)
    {
        try
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => br.MemberId == memberId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while getting borrow history for MemberId {MemberId}", memberId);
            throw;
        }
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByBookAsync(Guid bookId)
    {
        try
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => br.BookId == bookId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while getting borrow history for BookId {BookId}", bookId);
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
            _logger.LogError(ex, "An error occurred in BorrowRecordRepository while saving changes to the database.");
            throw;
        }
    }
}