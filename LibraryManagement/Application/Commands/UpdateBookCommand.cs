using MediatR;

namespace LibraryManagement.Application.Commands;

public class UpdateBookCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}