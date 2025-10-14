namespace LibraryManagement.Application.DTOs;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid LibraryId { get; set; }
}