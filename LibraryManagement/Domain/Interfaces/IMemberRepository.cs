using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Member> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Member>> GetByLibraryIdAsync(Guid libraryId);
    Task AddAsync(Member member);
    void Delete(Member member);
    Task<bool> SaveChangesAsync();
}