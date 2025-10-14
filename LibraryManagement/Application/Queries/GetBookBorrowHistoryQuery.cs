using LibraryManagement.Application.DTOs;
using MediatR;

namespace LibraryManagement.Application.Queries;

public class GetBookBorrowHistoryQuery : IRequest<IEnumerable<BorrowRecordDto>>
{
    public Guid BookId { get; }

    public GetBookBorrowHistoryQuery(Guid bookId) => BookId = bookId;
}