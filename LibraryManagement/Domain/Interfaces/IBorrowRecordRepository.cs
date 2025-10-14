using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces;

public interface IBorrowRecordRepository
{
    Task AddAsync(BorrowRecord borrowRecord);
    Task<BorrowRecord?> GetByIdAsync(Guid borrowId);
    Task<BorrowRecord?> GetActiveBorrowRecordForBookAsync(Guid bookId);
    Task<IEnumerable<BorrowRecord>> GetBorrowedBooksByMemberAsync(Guid memberId);
    Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByMemberAsync(Guid memberId);
    Task<IEnumerable<BorrowRecord>> GetBorrowHistoryByBookAsync(Guid bookId);
    Task<bool> SaveChangesAsync();
}