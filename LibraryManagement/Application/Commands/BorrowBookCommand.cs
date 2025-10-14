using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Commands;

public class BorrowBookCommand : IRequest<BorrowRecordDto>
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
}