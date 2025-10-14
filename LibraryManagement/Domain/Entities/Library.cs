namespace LibraryManagement.Domain.Entities;

public class Library
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BranchLocation { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<Member> Members { get; set; } = new List<Member>();
}