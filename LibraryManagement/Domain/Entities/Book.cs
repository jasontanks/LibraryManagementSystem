namespace LibraryManagement.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid LibraryId { get; set; }

    public Library? Library { get; set; }
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}