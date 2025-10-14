namespace LibraryManagement.Application.DTOs;

public class BorrowRecordDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
}