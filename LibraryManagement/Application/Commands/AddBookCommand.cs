using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Commands;

public class AddBookCommand : IRequest<BookDto>
{
    public string Title { get; set; } = string.Empty;
    public Guid LibraryId { get; set; }
}