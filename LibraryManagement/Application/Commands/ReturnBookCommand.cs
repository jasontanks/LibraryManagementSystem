using MediatR;

namespace LibraryManagement.Application.Commands;

public class ReturnBookCommand : IRequest<bool>
{
    public Guid BorrowId { get; init; }
}