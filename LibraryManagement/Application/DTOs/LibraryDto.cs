namespace LibraryManagement.Application.DTOs;

public class LibraryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BranchLocation { get; set; } = string.Empty;
}