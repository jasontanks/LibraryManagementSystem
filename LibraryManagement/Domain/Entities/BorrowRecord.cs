namespace LibraryManagement.Domain.Entities;

public class BorrowRecord
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Guid BookId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }

    public Member? Member { get; set; }
    public Book? Book { get; set; }
}