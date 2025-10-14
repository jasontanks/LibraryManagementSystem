namespace LibraryManagement.Application.DTOs;

public class MemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Guid LibraryId { get; set; }
}