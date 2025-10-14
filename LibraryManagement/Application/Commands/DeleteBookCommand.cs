using MediatR;

namespace LibraryManagement.Application.Commands;

public class DeleteBookCommand : IRequest<bool>
{
    public Guid BookId { get; set; }

    public DeleteBookCommand(Guid bookId)
    {
        BookId = bookId;
    }
}