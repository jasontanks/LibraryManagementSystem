namespace LibraryManagement.Domain.Entities;

public class Member
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Guid LibraryId { get; set; }

    public Library? Library { get; set; }
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}